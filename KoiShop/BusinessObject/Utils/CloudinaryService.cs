using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Utils
{
    public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        public CloudinaryService()
        {
            // Provide Cloudinary credentials (get from your Cloudinary dashboard)
            var account = new Account(
                "dwdph9tsd",    // Your Cloudinary cloud name
                "391774853411878",       // Your Cloudinary API key
                "nl_AONvwUc0Ap81iI__vzj6l5uQ"     // Your Cloudinary API secret
            );

            _cloudinary = new Cloudinary(account);
        }
        public async Task<string> UploadImageAsync(Stream fileStream, string fileName)
        {
            // Check if the stream is provided
            if (fileStream == null || fileStream.Length == 0)
            {
                throw new ArgumentException("No file stream provided or stream is empty.");
            }

            // Validate file extension (e.g., only allow .jpg, .jpeg, .png, .gif)
            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(fileName).ToLower();

            if (!validExtensions.Contains(fileExtension))
            {
                throw new InvalidOperationException("Invalid file format. Only .jpg, .jpeg, .png, .gif are allowed.");
            }

            // Create a public ID based on the original file name and timestamp
            var imageName = Path.GetFileNameWithoutExtension(fileName);
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var publicId = $"{imageName}_{timestamp}";

            // Create an upload parameter to specify the file and options
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, fileStream), // Use the provided stream and filename
                PublicId = publicId,
                Transformation = new Transformation().Width(500).Height(500).Crop("fill")
            };

            // Upload the image to Cloudinary
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            // Check if the upload was successful
            if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception($"Image upload failed. Cloudinary error: {uploadResult.Error?.Message}");
            }

            // Return the secure URL of the uploaded image
            return uploadResult.SecureUrl.AbsoluteUri;
        }

        public async Task<string> UploadImageFromUrlAsync(string imageUrl)
        {
            // Validate the URL
            if (!Uri.IsWellFormedUriString(imageUrl, UriKind.Absolute))
            {
                throw new ArgumentException("The provided image URL is not valid.");
            }

            // Create a public ID based on the image name and timestamp
            var imageName = Path.GetFileNameWithoutExtension(imageUrl); // Get image name without extension
            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var publicId = $"{imageName}_{timestamp}"; // Create a public ID based on the image name and timestamp

            using (var httpClient = new HttpClient())
            {
                // Download the image data
                var response = await httpClient.GetAsync(imageUrl);
                response.EnsureSuccessStatusCode();

                // Read the image content into a stream
                using (var stream = await response.Content.ReadAsStreamAsync())
                {
                    // Create upload parameters
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(imageUrl, stream), // Use the stream for the upload
                        PublicId = publicId,
                        Transformation = new Transformation().Width(500).Height(500).Crop("fill")
                    };

                    // Upload the image to Cloudinary
                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                    // Check if the upload was successful
                    if (uploadResult.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        throw new Exception($"Image upload failed. Cloudinary error: {uploadResult.Error?.Message}");
                    }

                    // Return the secure URL of the uploaded image
                    return uploadResult.SecureUrl.AbsoluteUri;
                }
            }
        }
    }
}
