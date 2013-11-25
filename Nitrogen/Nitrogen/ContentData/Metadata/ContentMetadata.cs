﻿/*
 *   Nitrogen - Halo Content API
 *   Copyright © 2013 The Nitrogen Authors. All rights reserved.
 * 
 *   This file is part of Nitrogen.
 *
 *   Nitrogen is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   Nitrogen is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with Nitrogen.  If not, see <http://www.gnu.org/licenses/>.
 */

using Nitrogen.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;

namespace Nitrogen.ContentData.Metadata
{
    /// <summary>
    /// Represents the metadata of a content.
    /// </summary>
    public class ContentMetadata
        : ISerializable<EndianStream>, ISerializable<BitStream>
    {
        private const int CommunityCategoryIndex = 25;

        private sbyte contentType;
        private long[] contentIds;
        private sbyte activity;
        private sbyte mode;
        private sbyte engine;
        private int map;
        private sbyte category;
        private Author creator, modifier;
        private DateTime dateCreated, dateModified;
        private string name, description;
        private uint screenshotNumber;
        private uint filmDuration;
        private sbyte variantIcon;
        private short hopperId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentMetadata"/> class with default values
        /// based on the specified content <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type of this content.</param>
        public ContentMetadata(ContentType type)
        {
            this.contentType = (sbyte)type;
            this.contentIds = new long[] { -1, -1, -1, -1 };
            this.category = -1;
            this.map = -1;
            this.creator = new Author();
            this.modifier = new Author();
            this.name = "Untitled";
            this.description = "This content was generated by Nitrogen.";
            this.variantIcon = -1;

            switch (type)
            {
                case ContentType.GameVariant:
                    this.mode = (sbyte)GameMode.PVP;
                    this.engine = (sbyte)GameEngine.PVP;
                    this.category = CommunityCategoryIndex;
                    break;

                case ContentType.MapVariant:
                    this.mode = (sbyte)GameMode.Forge;
                    this.engine = (sbyte)GameEngine.Forge;
                    break;

                // TODO: Add more stuff here
            }
        }

        /// <summary>
        /// Gets or sets the name of this content.
        /// </summary>
        public string Name
        {
            get { return this.name;}
            set
            {
                Contract.Requires<ArgumentNullException>(value != null);
                Contract.Requires<ArgumentException>(Encoding.BigEndianUnicode.GetByteCount(value) <= 256);

                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets the target map of this content.
        /// </summary>
        public int Map
        {
            get { return this.map; }
            set { this.map = value; }
        }

        /// <summary>
        /// Gets the type of this content.
        /// </summary>
        public ContentType ContentType
        {
            get { return (ContentType)this.contentType; }
            protected set { this.contentType = (sbyte)value; }
        }

        /// <summary>
        /// Gets the underlying game engine of this content.
        /// </summary>
        public GameEngine Engine
        {
            get { return (GameEngine)this.engine; }
            protected set { this.engine = (sbyte)value; }
        }

        #region ISerializable<EndianStream> Members

        public virtual void Serialize(EndianStream s)
        {
            s.Stream(ref this.contentType);
            s.Pad(3);
            s.Seek(4, SeekOrigin.Current); // Skip the file length (not sure how we'll handle this yet)
            s.Stream(this.contentIds, 0, this.contentIds.Length);
            s.Stream(ref this.activity);
            s.Stream(ref this.mode);
            s.Stream(ref this.engine);
            s.Pad(1);
            s.Stream(ref this.map);
            s.Stream(ref this.category);
            s.Pad(7);
            s.Stream(ref this.dateCreated);
            s.Serialize(this.creator);
            s.Stream(ref this.dateModified);
            s.Serialize(this.modifier);
            s.Stream(ref this.name, Encoding.BigEndianUnicode, 256);
            s.Stream(ref this.description, Encoding.BigEndianUnicode, 256);

            switch ((ContentType)contentType)
            {
                case ContentType.Screenshot:
                    s.Stream(ref this.screenshotNumber);
                    break;

                case ContentType.Film:
                case ContentType.FilmClip:
                    s.Stream(ref this.filmDuration);
                    break;

                case ContentType.GameVariant:
                    s.Stream(ref this.variantIcon);
                    s.Pad(24);
                    break;
            }

            if ((GameActivity)this.activity == GameActivity.Matchmaking)
                s.Stream(ref this.hopperId);
        }

        #endregion

        #region ISerializable<BitStream> Members

        public virtual void Serialize(BitStream s)
        {
            s.StreamPlusOne(ref this.contentType, 4);
            int temp = 0x7329;
            s.Stream(ref temp); // TODO: figure out how to handle the file length stuff
            s.Stream(this.contentIds, 0, this.contentIds.Length);
            s.Stream(ref this.activity, 2); // 3 bits in Halo: Reach hmm.. how will we handle this?
            s.Stream(ref this.mode, 3);
            s.Stream(ref this.engine, 3);
            s.Stream(ref this.map);
            s.Stream(ref this.category);
            s.Stream(ref this.dateCreated);
            s.Serialize(this.creator);
            s.Stream(ref this.dateModified);
            s.Serialize(this.modifier);
            s.Stream(ref this.name, Encoding.BigEndianUnicode);
            s.Stream(ref this.description, Encoding.BigEndianUnicode);

            switch ((ContentType)this.contentType)
            {
                case ContentType.Screenshot:
                    s.Stream(ref this.screenshotNumber);
                    break;

                case ContentType.Film:
                    s.Stream(ref this.filmDuration);
                    break;

                case ContentType.MapVariant:
                    // Stream nothing
                    break;

                default: // Game Variants
                    s.Stream(ref this.variantIcon);
                    break;
            }

            if ((GameActivity)this.activity == GameActivity.Matchmaking)
                s.Stream(ref this.hopperId);
        }

        #endregion
    }
}
