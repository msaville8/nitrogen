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

namespace Nitrogen.ContentData.MapVariants
{
    /// <summary>
    /// Represents a spherical boundary.
    /// </summary>
    public struct Sphere
        : IBoundary
    {
        private short radius;

        /// <summary>
        /// Gets or sets the radius of this sphere.
        /// </summary>
        public short Radius
        {
            get { return this.radius; }
            set { this.radius = value; }
        }

        public bool IsInBoundary(float x, float y, float z, IMapVariantObject o)
        {
            return
                (o.X >= x - radius && o.X <= x + radius) &&
                (o.Y >= y - radius && o.Y <= y + radius) &&
                (o.Z >= z - radius && o.Z <= z + radius);
        }

        #region IBoundary Members

        byte IBoundary.BoundaryIndex { get { return 1; } }

        public void Serialize(BitStream s)
        {
            s.Stream(ref this.radius);
        }

        #endregion
    }
}