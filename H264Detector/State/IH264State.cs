﻿/*
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

using Defraser.Interface;

namespace Defraser.Detector.H264.State
{
	internal interface IH264State
	{
		#region Properties
		/// <summary>SequenceState-level decoding state.</summary>
		ISequenceStates SequenceStates { get; }
		/// <summary>PictureState-level decoding state.</summary>
		IPictureStates PictureStates { get; }
		/// <summary>Slice-level decoding state.</summary>
		ISliceState SliceState { get; set; }
		/// <summary>The total number of succesfully PARSED headers, not the number of found startcodes.</summary>
		uint ParsedHeaderCount { get; set; }
		/// <summary>The total number of slices.</summary>
		uint SliceCount { get; set; }

		bool ByteStreamFormat { get; set; }
		NalUnitType NalUnitType { get; set; }
		bool IdrPicFlag { get; }
		uint NalRefIdc { get; set; }
		long ReferenceHeaderPosition { get; set; }
		int ReferenceHeadersTestCount { get; set; }
		IDataPacket ReferenceHeader { get; set; }
		#endregion Properties

		void Reset();
	}
}
