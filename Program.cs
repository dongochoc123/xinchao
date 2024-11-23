using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

class Program
{
    public static void Main(string[] args)
    {
        string botToken = "6591392740:AAFusEvzSo-0-VdYJGRBUrPtfp8jGsoNiqw";

        getMessage(botToken);

    }


    public static string SendVideoErr(string videoUrl, string text)
    {
        try
        {
            // Tải video về cùng thư mục với exe
            string localVideoPath = DownloadVideo(videoUrl);

            // Kiểm tra nếu video tải về thành công
            if (string.IsNullOrEmpty(localVideoPath) || !File.Exists(localVideoPath))
            {
                Console.WriteLine("Video không tồn tại.");
                return "";
            }

            string token = "6591392740:AAFusEvzSo-0-VdYJGRBUrPtfp8jGsoNiqw";
            string chatId = "@ongochoc123";
            string url = $"https://api.telegram.org/bot{token}/sendVideo";

            // Tạo một HttpClient để gửi tệp (tái sử dụng HttpClient)
            using (HttpClient client = new HttpClient())
            {
                using (var form = new MultipartFormDataContent())
                {
                    // Mở tệp video
                    using (var fileStream = new FileStream(localVideoPath, FileMode.Open, FileAccess.Read))
                    {
                        form.Add(new StreamContent(fileStream), "video", Path.GetFileName(localVideoPath));
                        form.Add(new StringContent(chatId), "chat_id");
                        form.Add(new StringContent(text), "caption");

                        // Gửi yêu cầu POST đồng bộ
                        HttpResponseMessage response = client.PostAsync(url, form).Result; // Dùng .Result để đồng bộ hóa

                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Video đã được gửi thành công!");
                            return "Ok";
                        }
                        else
                        {
                            Console.WriteLine($"Lỗi khi gửi video: {response.StatusCode}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi: {ex.Message}");
        }
        return "";
    }

    public static string DownloadVideo(string videoUrl)
    {
        try
        {
            // Lấy thư mục của tệp exe
            string localFolder = AppDomain.CurrentDomain.BaseDirectory;

            // Tạo tên tệp từ URL
            string fileName = Path.GetFileName(new Uri(videoUrl).AbsolutePath);
            string localPath = Path.Combine(localFolder, fileName); // Đường dẫn lưu tệp

            // Tải video đồng bộ
            using (HttpClient client = new HttpClient())
            {
                byte[] videoData = client.GetByteArrayAsync(videoUrl).Result;  // Tải video đồng bộ
                File.WriteAllBytes("video.mp4", videoData);  // Lưu video vào thư mục

                Console.WriteLine($"Video đã được tải về: {localPath}");
                return "";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi khi tải video: {ex.Message}");
            return null;
        }
    }


    public static string SendVideo(string videoUrl, string text)
    {
        try
        {
            string token = "6591392740:AAFusEvzSo-0-VdYJGRBUrPtfp8jGsoNiqw";
            string chatId = "@ongochoc123";
            string url = $"https://api.telegram.org/bot{token}/sendVideo";

            // Tạo một HttpWebRequest với phương thức POST
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";  // Sử dụng phương thức POST
            request.ContentType = "application/x-www-form-urlencoded"; // Content type

            // Dữ liệu cần gửi: video và caption
            string postData = $"chat_id={HttpUtility.UrlEncode(chatId)}&video={HttpUtility.UrlEncode(videoUrl)}&caption={HttpUtility.UrlEncode(text)}";

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;

            // Gửi dữ liệu qua request
            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }

            // Nhận phản hồi từ Telegram
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Console.WriteLine("Video đã được gửi thành công!");
                    return "Ok";
                }
                else
                {
                    Console.WriteLine($"Lỗi khi gửi video: {response.StatusCode}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Tải video!");
            return SendVideoErr(videoUrl, text);
        }
        return "";
    }


    public static string sendVideoMesss(string videoUrl, string text)
    {
        try
        {
            Console.WriteLine($"Send message");
            // Tạo URL API Telegram để gửi video
            string url = $"https://api.telegram.org/bot6591392740:AAFusEvzSo-0-VdYJGRBUrPtfp8jGsoNiqw/sendMessage?chat_id=@ongochoc123&text=" + text;

            // Tạo một HttpWebRequest với URL API Telegram
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET"; // Sử dụng phương thức GET

            // Gửi yêu cầu và nhận phản hồi
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                // Kiểm tra nếu yêu cầu thành công (HTTP 200 OK)
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return "Ok";
                }
                else
                {
                    Console.WriteLine($"Lỗi khi gửi video: {response.StatusCode}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Lỗi: {ex.Message}");
        }
        return "";
    }

    public static string getMessage(string botToken)
    {
        string list = "";
    cca:
        while (true)
        {
            // Tạo URL API Telegram để lấy các cập nhật (tin nhắn từ nhóm)
            string url = $"https://api.telegram.org/bot{botToken}/getUpdates?offset=-1&limit=1";

            try
            {
                // Tạo một HttpWebRequest với URL API Telegram
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET"; // Sử dụng phương thức GET

                // Gửi yêu cầu và nhận phản hồi
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    // Đọc phản hồi từ Telegram API
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string responseBody = reader.ReadToEnd();
                        string data = responseBody.Split(new[] { "\"update_id\":" }, StringSplitOptions.None)[1].Split(',')[0];
                        if (!list.Contains(data))
                        {
                            string url1 = responseBody.Split(new[] { "\"text\":\"" }, StringSplitOptions.None)[1].Split('"')[0];
                            if (url1.Contains("tiktok.com"))
                            {
                                sendVideoMesss("", "Tiến hành tải video =============================================================");
                                string urlVideo = test(url1);
                                if (urlVideo == "")
                                {
                                    Thread.Sleep(3000);
                                    goto cca;
                                }
                            }

                        }
                        list += data + "\n";


                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }

            System.Threading.Thread.Sleep(5000);
        }
        return "";
    }
    public static string test(string url)
    {
    cca:

        int index = 0;
        string proxyAddress = "216.185.48.182:50100"; // Thay đổi địa chỉ Proxy tại đây
        string proxyUsername = "uamesgames7857"; // Thay đổi username Proxy tại đây
        string proxyPassword = "RJ7b56ktAw"; // Thay đổi password Proxy tại đây

        try
        {
            // Tạo yêu cầu HTTP đến trang TikTok đầu tiên
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36";
            request.Timeout = 15000; // Timeout 15 giây

            // Cấu hình Proxy với xác thực
            WebProxy proxy = new WebProxy(proxyAddress);
            proxy.Credentials = new NetworkCredential(proxyUsername, proxyPassword); // Thêm xác thực Proxy
            request.Proxy = proxy;

            // Nhận phản hồi
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    // Đọc nội dung phản hồi
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string responseContent = reader.ReadToEnd();

                        try
                        {
                            string id = responseContent.Split(new[] { "{\"id\":\"" }, StringSplitOptions.None)[1].Split('"')[0];
                            Console.WriteLine($"ID Extracted: {id}");

                            // URL thứ hai với tham số id
                            Console.WriteLine($"Get url video");
                            string secondUrl = $"https://api22-normal-c-alisg.tiktokv.com/aweme/v1/feed/?aweme_id={id}&iid=7318518857994389254&device_id=7318517321748022790";

                            int maxRetries = 10;
                            int attempt = 0;

                            while (attempt < maxRetries)
                            {
                                try
                                {
                                    // Create the HttpWebRequest
                                    HttpWebRequest secondRequest = (HttpWebRequest)WebRequest.Create(secondUrl);
                                    secondRequest.Method = "GET";
                                    secondRequest.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/130.0.0.0 Safari/537.36";
                                    secondRequest.Timeout = 15000; // Timeout 15 seconds
                                    //secondRequest.Proxy = proxy; // Use the configured proxy

                                    // Get the response synchronously
                                    using (HttpWebResponse secondResponse = (HttpWebResponse)secondRequest.GetResponse())
                                    {
                                        if (secondResponse.StatusCode == HttpStatusCode.OK)
                                        {
                                            using (StreamReader secondReader = new StreamReader(secondResponse.GetResponseStream()))
                                            {
                                                Console.WriteLine($"Get url video 2");

                                                string secondResponseContent = secondReader.ReadToEnd();
                                                string data = secondResponseContent.Split(new[] { "video\":{\"play_addr\":{\"uri\":\"" }, StringSplitOptions.None)[1]
                                                                                     .Split(new[] { "\"url_list\":[\"" }, StringSplitOptions.None)[1]
                                                                                     .Split('"')[0];
                                                string text = secondResponseContent.Split(new[] { "\"desc\":\"" }, StringSplitOptions.None)[1]
                                                                                   .Split(new[] { "\",\"create_time" }, StringSplitOptions.None)[0];
                                                Console.WriteLine(data); // Log the response content
                                                return SendVideo(data, text);
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine($"Request failed with status code {secondResponse.StatusCode}");
                                            attempt++;
                                            if (attempt >= maxRetries)
                                            {
                                                return "Lỗi"; // Error message after max retries
                                            }
                                            Thread.Sleep(2000); // Blocking delay for retry
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Get url video lỗi");
                                    Console.WriteLine($"Error occurred: {ex.Message}");
                                    attempt++;
                                    if (attempt >= maxRetries)
                                    {
                                        return "Lỗi";
                                    }
                                    Thread.Sleep(2000); // Blocking delay for retry
                                }
                            }

                            return "Lỗi"; // Default failure return
                        }
                        catch (Exception ex)
                        {
                            index++;
                            if (index == 10)
                            {
                                return "Lỗi";
                            }
                            Thread.Sleep(2000);
                            goto cca;
                        }
                    }
                }
                else
                {
                    index++;
                    if (index == 10)
                    {
                        return "Lỗi";
                    }
                    Thread.Sleep(2000);

                    goto cca;
                }
            }
        }
        catch (WebException ex)
        {
            index++;
            if (index == 10)
            {
                return "Lỗi";
            }
            Thread.Sleep(2000);

            goto cca;
        }
        catch (Exception ex)
        {
            index++;
            if (index == 10)
            {
                return "Lỗi";
            }
            Thread.Sleep(2000);

            goto cca;
        }

        Console.ReadLine();
    }
}
