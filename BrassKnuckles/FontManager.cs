using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BrassKnuckles
{
    /// <summary>
    /// Base interface for classes that manage SpriteFont instances.
    /// </summary>
    public interface IFontManager
    {
        #region Properties

        /// <summary>
        /// Count of SpriteFont instances contained in the cache.
        /// </summary>
        int Count { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new SpriteFont instance using the provided asset name and adds it to the cache, using the name provided.
        /// </summary>
        /// <param name="name">Name to use as a key in the cache.</param>
        /// <param name="assetName">Asset name, relative to the loader root directory, without the .xnb extension.</param>
        void AddFont(string name, string assetName);

        /// <summary>
        /// Adds a SpriteFont instance to the cache, with the specified name.
        /// </summary>
        /// <param name="name">Name to use as a key in the cache.</param>
        /// <param name="font">Font to be added to the cache.</param>
        /// <remarks>If an existing font is using the same name as the name provided, the existing texture will be replaced.</remarks>
        void AddFont(string name, SpriteFont font);

        /// <summary>
        /// Removes the font with the specified name. Does not remove fonts that are aliases to the same source.
        /// </summary>
        /// <param name="name">Name of the font to remove.</param>
        /// <returns>True if the font was found and removed, false if not.</returns>
        bool RemoveFont(string name);

        /// <summary>
        /// Removes all fonts from the cache.
        /// </summary>
        void ClearFonts();

        /// <summary>
        /// Gets the font with the specified name.
        /// </summary>
        /// <param name="name">Name of the font to retrieve.</param>
        /// <returns>The SpriteFont instance with the name specified, or null if not found.</returns>
        SpriteFont GetFont(string name);

        /// <summary>
        /// Checks if a font with the specified name exists in the cache.
        /// </summary>
        /// <param name="name">Name of the font to look for.</param>
        /// <returns>True if the cache contains a font with the specified name, false if not.</returns>
        bool ContainsFont(string name);

        #endregion
    }

    /// <summary>
    /// Manages a cache of SpriteFont instances to use for displaying text.
    /// </summary>
    public class FontManager : IFontManager
    {
        #region Private fields

        private ContentManager _contentManager;

        #endregion

        #region Properties

        /// <summary>
        /// Font cache.
        /// </summary>
        protected Dictionary<string, SpriteFont> Fonts { get; private set; }

        /// <summary>
        /// Gets the number of SpriteFont instances contained in the cache.
        /// </summary>
        public int Count
        {
            get { return Fonts.Count; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new FontManager instance.
        /// </summary>
        /// <param name="contentManager">ContentManager instance to use.</param>
        public FontManager(ContentManager contentManager)
        {
            Fonts = new Dictionary<string, SpriteFont>();
            _contentManager = contentManager;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates a new SpriteFont instance using the provided asset name and adds it to the cache, using the name provided.
        /// </summary>
        /// <param name="name">Name to use as a key in the cache.</param>
        /// <param name="assetName">Asset name, relative to the loader root directory, without the .xnb extension.</param>
        public void AddFont(string name, string assetName)
        {
            SpriteFont font = _contentManager.Load<SpriteFont>(assetName);
            AddFont(name, font);
        }

        /// <summary>
        /// Adds a SpriteFont instance to the cache, with the specified name.
        /// </summary>
        /// <param name="name">Name to use as a key in the cache.</param>
        /// <param name="font">Font to be added to the cache.</param>
        /// <remarks>If an existing font is using the same name as the name provided, the existing texture will be replaced.</remarks>
        public void AddFont(string name, SpriteFont font)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            if (font == null)
            {
                throw new ArgumentNullException("font");
            }

            if (Fonts.ContainsKey(name))
            {
                Fonts[name] = font;
            }
            else
            {
                Fonts.Add(name, font);
            }
        }

        /// <summary>
        /// Removes the font with the specified name. Does not remove fonts that are aliases to the same source.
        /// </summary>
        /// <param name="name">Name of the font to remove.</param>
        /// <returns>True if the font was found and removed, false if not.</returns>
        public bool RemoveFont(string name)
        {
            return Fonts.Remove(name);
        }

        /// <summary>
        /// Removes all fonts from the cache.
        /// </summary>
        public void ClearFonts()
        {
            Fonts.Clear();
        }

        /// <summary>
        /// Gets the font with the specified name.
        /// </summary>
        /// <param name="name">Name of the font to retrieve.</param>
        /// <returns>The SpriteFont instance with the name specified, or null if not found.</returns>
        public SpriteFont GetFont(string name)
        {
            if (Fonts.ContainsKey(name))
            {
                return Fonts[name];
            }

            return null;
        }

        /// <summary>
        /// Checks if a font with the specified name exists in the cache.
        /// </summary>
        /// <param name="name">Name of the font to look for.</param>
        /// <returns>True if the cache contains a font with the specified name, false if not.</returns>
        public bool ContainsFont(string name)
        {
            return Fonts.ContainsKey(name);
        }

        #endregion
    }
}
