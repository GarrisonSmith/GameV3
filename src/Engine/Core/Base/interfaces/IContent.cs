﻿using System;

namespace Engine.Core.Base.interfaces
{
    /// <summary>
    /// Represents content. 
    /// </summary>
    public interface IContent : IDisposable
    {
        /// <summary>
        /// Gets or sets the guid.
        /// </summary>
        Guid Guid { get; }
    }
}
