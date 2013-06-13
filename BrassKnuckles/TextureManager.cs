using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace BrassKnuckles
{
    /// <summary>
    /// Base interface for classes that manage Texture2D instances.
    /// </summary>
    public interface ITextureManager
    {
        #region Properties

        /// <summary>
        /// Count of Texture2D instances contained in the cache.
        /// </summary>
        int Count { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new Texture2D instance using the provided asset name and adds it to the cache, using the name provided.
        /// </summary>
        /// <param name="name">Name to use as a key in the cache.</param>
        /// <param name="assetName">Asset name, relative to the loader root directory, without the .xnb extension.</param>
        void AddTexture(string name, string assetName);

        /// <summary>
        /// Adds a Texture2D instance to the cache, with the specified name.
        /// </summary>
        /// <param name="name">Name to use as a key in the cache.</param>
        /// <param name="texture">Texture to be added to the cache.</param>
        /// <remarks>If a texture in the cache uses the same source file, an alias to the existing texture will be created instead of adding a duplicate texture. Also, if an existing texture
        /// is using the same name as the name provided, the existing texture will be replaced.</remarks>
        void AddTexture(string name, Texture2D texture);

        /// <summary>
        /// Removes the texture with the specified name. Does not remove textures that are aliases to the same source.
        /// </summary>
        /// <param name="name">Name of the texture to remove.</param>
        /// <returns>True if the texture was found and removed, false if not.</returns>
        bool RemoveTexture(string name);

        /// <summary>
        /// Removes all textures from the cache.
        /// </summary>
        void ClearTextures();

        /// <summary>
        /// Gets the texture with the specified name.
        /// </summary>
        /// <param name="name">Name of the texture to retrieve.</param>
        /// <returns>The Texture2D instance with the name specified, or null if not found.</returns>
        Texture2D GetTexture(string name);

        /// <summary>
        /// Checks if a texture with the specified name exists in the cache.
        /// </summary>
        /// <param name="name">Name of the texture to look for.</param>
        /// <returns>True if the cache contains a texture with the specified name, false if not.</returns>
        bool ContainsTexture(string name);

        #endregion
    }

    /// <summary>
    /// Manages a cache of Texture2D instances to use with sprites and other game graphics.
    /// </summary>
    public class TextureManager : ITextureManager
    {
        #region Private fields

        private ContentManager _contentManager;

        #endregion

        #region Properties

        /// <summary>
        /// Texture cache.
        /// </summary>
        protected Dictionary<string, Texture2D> Textures { get; private set; }

        /// <summary>
        /// Gets the number of Texture2D instances contained in the cache.
        /// </summary>
        public int Count
        {
            get { return Textures.Count; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new TextureManager instance.
        /// </summary>
        /// <param name="contentManager">ContentManager instance to use.</param>
        public TextureManager(ContentManager contentManager)
        {
            Textures = new Dictionary<string, Texture2D>();
            _contentManager = contentManager;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates a new Texture2D instance using the provided asset name and adds it to the cache, using the name provided.
        /// </summary>
        /// <param name="name">Name to use as a key in the cache.</param>
        /// <param name="assetName">Asset name, relative to the loader root directory, and not including the .xnb extension.</param>
        public void AddTexture(string name, string assetName)
        {
            Texture2D texture = _contentManager.Load<Texture2D>(assetName);
            texture.Name = assetName;
            AddTexture(name, texture);
        }

        /// <summary>
        /// Adds a Texture2D instance to the cache, with the specified name.
        /// </summary>
        /// <param name="name">Name to use as a key in the cache.</param>
        /// <param name="texture">Texture to be added to the cache.</param>
        /// <remarks>If a texture in the cache uses the same source file, an alias to the existing texture will be created instead of adding a duplicate texture. Also, if an existing texture
        /// is using the same name as the name provided, the existing texture will be replaced.</remarks>
        public void AddTexture(string name, Texture2D texture)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            if (texture == null)
            {
                throw new ArgumentNullException("texture");
            }

            // Look for a texture where the texture source name is the same as the source name of the texture being added,
            // if the source name of the texture being added is not null or empty. Matching source names means the source files are the same.
            Texture2D original = null;
            if (!string.IsNullOrEmpty(texture.Name))
            {
                original = (from t in Textures.Values
                            where t.Name == texture.Name
                            select t).FirstOrDefault();
            }
            // No match found, so this is a new texture.
            if (original == null)
            {
                if (Textures.ContainsKey(name))
                {
                    Textures[name] = texture;
                }
                else
                {
                    Textures.Add(name, texture);
                }
            }
            // Match found, so this is an alias to a texture already added
            else
            {
                if (Textures.ContainsKey(name))
                {
                    Textures[name] = original;
                }
                else
                {
                    Textures.Add(name, original);
                }
            }
        }

        /// <summary>
        /// Removes the texture with the specified name. Does not remove textures that are aliases to the same source.
        /// </summary>
        /// <param name="name">Name of the texture to remove.</param>
        /// <returns>True if the texture was found and removed, false if not.</returns>
        public bool RemoveTexture(string name)
        {
            return Textures.Remove(name);
        }

        /// <summary>
        /// Removes all textures from the cache.
        /// </summary>
        public void ClearTextures()
        {
            Textures.Clear();
        }

        /// <summary>
        /// Gets the texture with the specified name.
        /// </summary>
        /// <param name="name">Name of the texture to retrieve.</param>
        /// <returns>The Texture2D instance with the name specified, or null if not found.</returns>
        public Texture2D GetTexture(string name)
        {
            if (Textures.ContainsKey(name))
            {
                return Textures[name];
            }

            return null;
        }

        /// <summary>
        /// Checks if a texture with the specified name exists in the cache.
        /// </summary>
        /// <param name="name">Name of the texture to look for.</param>
        /// <returns>True if the cache contains a texture with the specified name, false if not.</returns>
        public bool ContainsTexture(string name)
        {
            return Textures.ContainsKey(name);
        }

        #endregion
    }
}
