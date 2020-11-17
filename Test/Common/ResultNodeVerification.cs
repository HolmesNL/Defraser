/*
 * Copyright (c) 2006-2020, Netherlands Forensic Institute
 * All Rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. Neither the name of the Netherlands Forensic Institute nor the names 
 *    of its contributors may be used to endorse or promote products derived
 *    from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE
 * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
 * OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
 * OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
 * SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Defraser.DataStructures;
using Defraser.Interface;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.SyntaxHelpers;

namespace Defraser.Test.Common
{
	public class ResultNodeVerification
	{
		private readonly IResultNode _resultNode;
		private readonly String _description;

		public ResultNodeVerification(IResultNode resultNode, String description)
		{
			Assert.IsNotNull(resultNode);
			_resultNode = resultNode;
			_description = resultNode.Name + "-" + description;
		}

		public ResultNodeVerification HeaderCount(Constraint count)
		{
			return HeaderCount(count, String.Empty);
		}

		public ResultNodeVerification HeaderCountEquals(int count)
		{
			return HeaderCountEquals(count, String.Empty);
		}

		public ResultNodeVerification HeaderCount(Constraint count, string description)
		{
			var extraDescription = String.IsNullOrEmpty(description) ? "" : "(" + description + ") ";
			Assert.That(_resultNode.Children.Count, count, "Number of headers "+extraDescription+"within " + _description);
			return this;
		}

		public ResultNodeVerification HeaderCountEquals(int count, string description)
		{
			return HeaderCount(Is.EqualTo(count), description);
		}

		public ResultNodeVerification VerifyChild(int index)
		{
			Assert.That(_resultNode.Children.Count,Is.AtLeast(index+1),"No child available to verify.");
			var child = _resultNode.Children[index];
			string descrText = DescribeChild(child);
			return new ResultNodeVerification(child, "child " + index + " " + descrText);
		}

		private string DescribeChild(IResultNode child)
		{
			return TestFramework.GetDescriptionText(child);
		}

		public ResultNodeVerification Name(Constraint name)
		{
			Assert.That(_resultNode.Name, name, "Name of " + _description);
			return this;
		}

		public KeyFrameVerification IsKeyFrame()
		{
			Bitmap bitmap = TestFFmpegSetup.FFmpegManager.FrameConvertor.FrameToBitmap(_resultNode);
			Assert.That(bitmap, Is.Not.Null, "Keyframe conversion failed (FFmpegManager).");
			return new KeyFrameVerification(bitmap, "Keyframe generated by ffmpeg from " + DescribeChild(_resultNode));
		}

		public ResultNodeVerification NameEquals(string expectedName)
		{
			return Name(Is.EqualTo(expectedName));
		}

		public ResultNodeVerification StartOffset(Constraint offset)
		{
			Assert.That(_resultNode.StartOffset, offset, "Offset of " + _description);
			return this;
		}

		public ResultNodeVerification StartOffsetEquals(long dataStart)
		{
			return StartOffset(Is.EqualTo(dataStart));
		}

		public ResultNodeVerification Length(Constraint length)
		{
			Assert.That(_resultNode.Length, length, "Length of " + _description);
			//TODO: Assert.That(_resultNode.EndOffset - _resultNode.StartOffset, Is.EqualTo(_resultNode.Length), "EndOffset-StartOffset=length");
			return this;
		}

		public ResultNodeVerification LengthEquals(long length)
		{
			return Length(Is.EqualTo(length));
		}

		public ResultNodeVerification IsRoot()
		{
			return Name(Is.EqualTo("Root"));
		}

		public ResultAttributeVerification VerifyAttribute(string attributeName)
		{
			return new ResultAttributeVerification(_resultNode.FindAttributeByName(attributeName), "Attribute (" + attributeName + ") of " + _description);
		}

		public ResultNodeVerification VerifyAttribute(string attributeName, object value)
		{
			new ResultAttributeVerification(_resultNode.FindAttributeByName(attributeName), "Attribute (" + attributeName + ") of " + _description).Value(Is.EqualTo(value));
			return this;
		}

		public ResultNodeVerification Last()
		{
			var node = Last(_resultNode);
			var descr = DescribeChild(node);
			return new ResultNodeVerification(node, "last child " + descr);
		}

		private static IResultNode Last(IResultNode result)
		{
			if (result.Children.Count > 0)
			{
				return Last(result.Children[result.Children.Count - 1]);
			}
			return result;
		}

		/// <summary>
		/// Finds all descendents that are keyframes : all resultnodes in the tree under this resultnode that are keyframes according to the used detecor.
		/// </summary>
		public IEnumerable<ResultNodeVerification> VerifyKeyFrames()
		{
			return FindKeyFrame(_resultNode).Select((node, index) => new ResultNodeVerification(node, "child " + index + " " + DescribeChild(node)));
		}

		private static IEnumerable<IResultNode> FindKeyFrame(IResultNode node)
		{
			if (node.Detectors.Any(d => node.IsKeyframe()))
			{
				yield return node;
			}

			foreach (IResultNode childNode in node.Children)
			{
				foreach (IResultNode keyFrameNode in FindKeyFrame(childNode))
					yield return keyFrameNode;
			}
			yield break;
		}

		public ResultNodeVerification VerifyOnlyChild(string nodeName)
		{
			return HeaderCountEquals(1, "one child (named " + nodeName + ")").VerifyChild(0).NameEquals(nodeName);
		}

		public void IsNoKeyFrame()
		{
			Bitmap bitmap = TestFFmpegSetup.FFmpegManager.FrameConvertor.FrameToBitmap(_resultNode);
			Assert.That(bitmap, Is.Null, "Expected Keyframe conversion to fail (FFmpegManager).");
		}
	}
}