﻿/*
 * Copyright (c) 2006-2020, Netherlands Forensic Institute
 * All Rights Reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 * 1. Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * 3. Neither the name of the Institute nor the names of its contributors
 *    may be used to endorse or promote products derived from this software
 *    without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE INSTITUTE AND CONTRIBUTORS ``AS IS'' AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED.  IN NO EVENT SHALL THE INSTITUTE OR CONTRIBUTORS BE LIABLE
 * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS
 * OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY
 * OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
 * SUCH DAMAGE.
 */

using Defraser.Detector.Common;

namespace Defraser.Detector.Asf
{
	internal class LengthTypeFlags : CompositeAttribute<DataPacket.LAttribute, string, AsfParser>
	{
		public enum LAttribute
		{
			MultiplePayloadsPresent,
			SequenceType,
			PaddingLengthType,
			PacketLengthType,
			ErrorCorrectionPresent,
		}

		private byte _lengthTypeFlags;

		internal bool MultiplePayloadsPresent { get; private set; }
		internal LengthType PacketLengthType { get; private set; }
		internal LengthType SequenceType { get; private set; }
		internal LengthType PaddingLengthType { get; private set; }

		public LengthTypeFlags(byte flags)
			: base(DataPacket.LAttribute.LengthTypeFlags, string.Empty, "{0}")
		{
			_lengthTypeFlags = flags;
		}

		public override bool Parse(AsfParser parser)
		{
			TypedValue = _lengthTypeFlags.ToString();

			MultiplePayloadsPresent = (_lengthTypeFlags & 1) != 0;
			Attributes.Add(new FormattedAttribute<LAttribute, bool>(LAttribute.MultiplePayloadsPresent, MultiplePayloadsPresent));

			SequenceType = (LengthType)((_lengthTypeFlags >>= 1) & 3);
			if (SequenceType != LengthType.ValueNotPresent) Valid = false;
			Attributes.Add(new FormattedAttribute<LAttribute, string>(LAttribute.SequenceType, SequenceType.PrettyPrint()));

			PaddingLengthType = (LengthType)((_lengthTypeFlags >>= 2) & 3);
			Attributes.Add(new FormattedAttribute<LAttribute, string>(LAttribute.PaddingLengthType, PaddingLengthType.PrettyPrint()));

			PacketLengthType = (LengthType)((_lengthTypeFlags >>= 2) & 3);
			Attributes.Add(new FormattedAttribute<LAttribute, string>(LAttribute.PacketLengthType, PacketLengthType.PrettyPrint()));

			bool errorCorrectionPresent = ((_lengthTypeFlags >>= 2) & 1) != 0;
			Attributes.Add(new FormattedAttribute<LAttribute, bool>(LAttribute.ErrorCorrectionPresent, errorCorrectionPresent));

			return Valid;
		}
	}
}