This files WebPConfigurationModule.cs, WebPCustomFormat.cs, WebPImageFormatDetector.cs
were added because imagesharp not fully supports webp formatting
When it will be fully supported:
    1. remove files: WebPConfigurationModule.cs, WebPCustomFormat.cs, WebPImageFormatDetector.cs
    2. remove Shorthand.ImageSharp.WebP from nuget packages
    3. Replace this options declaration
           options.Configuration = new Configuration(
                                new PngConfigurationModule(),
                                new JpegConfigurationModule(),
                                new BmpConfigurationModule(),
                                new GifConfigurationModule(),
                                new TgaConfigurationModule(),
                                new WebPConfigurationModule());
        with options.Configuration = Configuration.Default;
        at MvcConfiguration file services.AddImageSharp section
        
    BEWARE. the webp images quality is set in WebPConfigurationModule.cs file, when removing this you will need to
    set quality in img tag for each image or find a way to set it in the configuration
    
    
Install libwebp on linux
sudo apt-get update -y && sudo apt-get install -y libwebp-dev