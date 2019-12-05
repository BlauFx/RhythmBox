using System.Net.Http;
using System.Threading.Tasks;

namespace RhythmBox.Window.pending_files
{
    public class Updater
    {
        private HttpClient client = new HttpClient();
        private HttpResponseMessage response;
        private string responseStr = string.Empty;

        public async Task<bool> SearchAsyncForUpdates()
        {
            try
            {
                //response = await client.GetAsync("URL");
                ////response.EnsureSuccessStatusCode();
                //responseStr = await response.Content.ReadAsStringAsync();
            }
            finally
            {
                response.Dispose();
                client.Dispose();
            }

            return CheckNewVersionAvailable();
        }

        private bool CheckNewVersionAvailable()
        {
            if (GetCurrentVersion() != "0.0.0")
            {
                return true;
            }

            return false;
        }

        private string GetCurrentVersion()
        {
            return "0.0.0";
        }
    }
}
