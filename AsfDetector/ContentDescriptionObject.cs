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
	class ContentDescriptionObject : AsfObject
	{
		public new enum Attribute
		{
			TitleLength,
			AuthorLength,
			CopyrightLength,
			DescriptionLength,
			RatingLength,
			Title,
			Author,
			Copyright,
			Description,
			Rating
		}

		public ContentDescriptionObject(AsfObject previousObject)
			: base(previousObject, AsfObjectName.ContentDescriptionObject)
		{
		}

		public override bool Parse(AsfParser parser)
		{
			if (!base.Parse(parser)) return false;

			short titleLength = parser.GetShort(Attribute.TitleLength);
			short authorLength = parser.GetShort(Attribute.AuthorLength);
			short copyrightLength = parser.GetShort(Attribute.CopyrightLength);
			short descriptionLength = parser.GetShort(Attribute.DescriptionLength);
			short ratingLength = parser.GetShort(Attribute.RatingLength);

			parser.GetUnicodeString(Attribute.Title,(titleLength/2));
			parser.GetUnicodeString(Attribute.Author, (authorLength/2));
			parser.GetUnicodeString(Attribute.Copyright,(copyrightLength/2));
			parser.GetUnicodeString(Attribute.Description,(descriptionLength/2));
			parser.GetUnicodeString(Attribute.Rating,(ratingLength / 2));

			return Valid;
		}
	}
}