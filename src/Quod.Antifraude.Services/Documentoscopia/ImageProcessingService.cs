using SixLabors.ImageSharp;
using Img = SixLabors.ImageSharp.Image;
using SixLabors.ImageSharp.Processing;
// (remova qualquer `using System.Net.Mime;` se existir)

public class ImageProcessingService
{
    public string PreprocessImage(string inputPath, string outputPath)
    {
        // “fully qualified” remove qualquer ambiguidade
        using var img = Img.Load(inputPath);
        img.Mutate(x => x
            .Grayscale()
            .Contrast(1.5f)
            .BinaryThreshold(0.5f)
        );
        img.Save(outputPath);
        return outputPath;
    }
}
