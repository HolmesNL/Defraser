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

namespace Defraser.Detector.Asf
{
	class FilePropertiesObject : AsfObject
	{
		public new enum Attribute
		{
			FileID,
			FileSize,
			Date,
			DataPacketsCount,
			PlayDuration,
			SendDuration,
			Preroll,
			Flags,
			MinimumDataPacketSize,
			MaximumDataPacketSize,
			MaximumBitrate
		}

		public FilePropertiesObject(AsfObject previousObject)
			: base(previousObject, AsfObjectName.FilePropertiesObject)
		{
		}

		public override bool Parse(AsfParser parser)
		{
			if (!base.Parse(parser)) return false;
			
			parser.GetGuid(Attribute.FileID);
			parser.GetLong(Attribute.FileSize);
			parser.GetLongDateTime(Attribute.Date);
			parser.GetLong(Attribute.DataPacketsCount);
			parser.GetLongTime(Attribute.PlayDuration, TimeUnit.HundredNanoSeconds);
			parser.GetLongTime(Attribute.SendDuration, TimeUnit.HundredNanoSeconds);
			parser.GetLongTime(Attribute.Preroll, TimeUnit.Milliseconds);
			parser.GetInt(Attribute.Flags);
			parser.DataPacketLength = parser.GetInt(Attribute.MinimumDataPacketSize);
			parser.GetInt(Attribute.MaximumDataPacketSize);
			parser.GetInt(Attribute.MaximumBitrate);

			return Valid;
		}
	}
}