// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("40tr0iiC5Eg7RFWRCruEwNSM0okq/8sguZ2zAM5/bgZlEH6Z7OJHwo9UQJogbzsApMnKmg28Os2QgM0olFu4MYx+jmwL3lW1QnNtVO4YqFDAzsNFTSM/kVW6xjzcMMs6m0JPMq/QlpeZKM0eaZRzNhM1lPJoBTQjvBuzFXbzH9at38oHHwKGbeYn0S2xMjwzA7EyOTGxMjIznJG91L2qBQOxMhEDPjU6GbV7tcQ+MjIyNjMwGE8CUwm7IHP15fCzVOGn/uP72H8PJ94RUp8mmvlo4d4vbS3A1Q5LJoDuKUSwaIMWcgCNZaQDfhCNWbiD8WgKVbLqAZjWi/Ug50chkgtjL5B3yd21Wf2vsbGOAa9U5J4EizcauRHMTTU5xsVy/jEwMjMy");
        private static int[] order = new int[] { 7,4,9,12,6,5,10,10,10,12,12,11,12,13,14 };
        private static int key = 51;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
