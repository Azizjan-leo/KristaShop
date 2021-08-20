using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KristaShop.Common.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.Web;
using SixLabors.ImageSharp.Web.Providers;
using SixLabors.ImageSharp.Web.Resolvers;

namespace KristaShop.WebUI.Infrastructure {
    public class CustomPhysicalFileSystemProvider : IImageProvider {
        /// <summary>
        ///     The file provider abstraction.
        /// </summary>
        private readonly IFileProvider _fileProvider;

        /// <summary>
        ///     Contains various format helper methods based on the current configuration.
        /// </summary>
        private readonly FormatUtilities _formatUtilities;

        /// <inheritdoc />
        public ProcessingBehavior ProcessingBehavior => ProcessingBehavior.CommandOnly;

        /// <inheritdoc />
        public Func<HttpContext, bool> Match { get; set; } = _ => true;

        /// <inheritdoc />
        public bool IsValidRequest(HttpContext context) {
            return _formatUtilities.GetExtensionFromUri(context.Request.GetDisplayUrl()) != null;
        }
        
        /// <summary>
        ///     Initializes a new instance of the <see cref="CustomPhysicalFileSystemProvider" /> class.
        /// </summary>
        /// <param name="environment">The environment used by this middleware.</param>
        /// <param name="formatUtilities">Contains various format helper methods based on the current configuration.</param>
        public CustomPhysicalFileSystemProvider(
            IWebHostEnvironment environment,
            FormatUtilities formatUtilities,
            IOptions<GlobalSettings> options) {

            if (environment.IsDevelopment()) {
                //Include modules wwwroot folder in development
                var modulesRootPath = ModulesDeclaration.GetAssemblies().Select(assembly =>
                    Path.GetFullPath(Path.Combine(environment.ContentRootPath, "..", assembly.GetName().Name, "wwwroot")));
                var providers = new List<PhysicalFileProvider> {new(options.Value.FilesDirectoryPath)};
                providers.AddRange(modulesRootPath.Select(x => new PhysicalFileProvider(x)));
                _fileProvider = new CompositeFileProvider(providers);
            } else {
                _fileProvider = new PhysicalFileProvider(options.Value.FilesDirectoryPath);
            }

            _formatUtilities = formatUtilities;
        }

        /// <inheritdoc />
        public Task<IImageResolver> GetAsync(HttpContext context) {
            // Path has already been correctly parsed before here.
            var fileInfo = _fileProvider.GetFileInfo(context.Request.Path.Value);

            try {
                // Check to see if the file exists.
                if (!fileInfo.Exists) return Task.FromResult<IImageResolver>(null);

                var metadata = new ImageMetadata(fileInfo.LastModified.UtcDateTime, fileInfo.Length);
                return Task.FromResult<IImageResolver>(new PhysicalFileSystemResolver(fileInfo, metadata));
            } catch (Exception ex) {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}