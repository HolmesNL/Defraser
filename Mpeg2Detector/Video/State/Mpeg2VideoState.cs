/*
 * Copyright (c) 2006-2020, Netherlands Forensic Institute
 * All rights reserved.
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
using Defraser.Interface;

namespace Defraser.Detector.Mpeg2.Video.State
{
	internal sealed class Mpeg2VideoState : IMpeg2VideoState
	{
		private readonly IMpeg2VideoConfiguration _configuration;
		private readonly ISequenceState _sequenceState;
		private readonly IPictureState _pictureState;
		private readonly ISliceState _sliceState;

		#region Properties
		public IMpeg2VideoConfiguration Configuration { get { return _configuration; } }
		public CodecID MpegFormat { get; set; }
		public ISequenceState Sequence { get { return _sequenceState; } }
		public IPictureState Picture { get { return _pictureState; } }
		public ISliceState Slice { get { return _sliceState; } }
		public bool SeenGop { get; set; }
		public uint StartCode { get; set; }
		public string FirstHeaderName { get; set; }
		public string LastHeaderName { get; set; }
		public uint LastHeaderZeroByteStuffing { get; set; }
		public uint ParsedHeaderCount { get; set; }
		public uint InvalidSliceCount { get; set; }
		public uint ValidSliceCount { get; set; }
		public bool IsFragmented { get; set; }
		public IDataPacket ReferenceHeader { get; set; }
		public long ReferenceHeaderPosition { get; set; }
		public bool ReferenceHeadersTested { get; set; }
		#endregion

		public Mpeg2VideoState(IMpeg2VideoConfiguration configuration, ISequenceState sequenceState, IPictureState pictureState, ISliceState sliceState)
		{
			_configuration = configuration;
			_sequenceState = sequenceState;
			_pictureState = pictureState;
			_sliceState = sliceState;

			Reset();
		}

		public void Reset()
		{
			Sequence.Reset();
			Picture.Reset();
			Slice.Reset();

			MpegFormat = CodecID.Unknown;
			SeenGop = false;
			StartCode = 0;
			FirstHeaderName = null;
			LastHeaderName = null;
			LastHeaderZeroByteStuffing = 0;
			ParsedHeaderCount = 0;
			InvalidSliceCount = 0;
			ValidSliceCount = 0;
			IsFragmented = false;
			ReferenceHeader = null;
			ReferenceHeaderPosition = 0L;
			ReferenceHeadersTested = false;
		}

		public bool IsMpeg2()
		{
			return MpegFormat == CodecID.Mpeg2Video; // Note: Returns 'false' if uninitialized (unknown)!
		}
	}
}
