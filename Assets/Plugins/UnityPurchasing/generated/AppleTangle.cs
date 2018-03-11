#if UNITY_ANDROID || UNITY_IPHONE || UNITY_STANDALONE_OSX || UNITY_TVOS
// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class AppleTangle
    {
        private static byte[] data = System.Convert.FromBase64String("J5UTrCeVFLS3FBUWFRUWFScaER5zIjQCXAJOCqSD4OGLidhHrdZPR2V2dGN+dHI3ZGN2Y3J6cnljZDknMSczERRCExwEClZnZ3tyN1RyZWMSFxSVFhgXJ5UWHRWVFhYX84a+HmBgOXZnZ3tyOXR4ejh2Z2d7cnR2M/X8xqBnyBhS9jDd5npv+vCiAABne3I3VHJlY35xfnR2Y354eTdWYhEnGBEUQgoEFhboExInFBYW6CcKeXM3dHh5c35jfnh5ZDd4cTdiZHIRFEIKGRMBEwM8x35Qg2Ee6eN8mjdUVieVFjUnGhEePZFfkeAaFhYWExEEFUJEJgQnBhEUQhMdBB1WZ2ciJSYjJyQhTQAaJCInJScuJSYjJ3V7cjdkY3Z5c3ZlczdjcmV6ZDd2fnF+dHZjfnh5N1ZiY394ZX5jbiYqMXA3nSR94BqV2Mn8tDjuRH1Mc17PYYgkA3K2YIPeOhUUFhcWtJUWOVex4FBaaB9JJwgRFEIKNBMPJwEQ+2oulJxEN8Qv06aojVgdfOg86wEnAxEUQhMUBBpWZ2d7cjdFeHhjIY5bOm+g+puMy+RgjOVhxWAnWNaiLbrjGBkXhRymNgE5Y8IrGsx1AaAMqoRVMwU90BgKoVqLSXTfXJcAJwYRFEITHQQdVmdne3I3Xnl0OSZuN3ZkZGJ6cmQ3dnR0cmdjdnl0cqYnT/tNEyWbf6SYCslyZOhwSXKrqeNkjPnFcxjcblgjz7Up7m/ofN+CiW0bs1CcTMMBICTc0xha2QN+xjd2eXM3dHJlY35xfnR2Y354eTdnbSeVFmEnGREUQgoYFhboExMUFRY4J5bUER88ERYSEhAVFSeWoQ2WpJhklnfRDEweOIWl71Nf53cviQLicJgfozfg3Ls7N3hnoSgWJ5ugVNjXdCRg4C0QO0H8zRg2Gc2tZA5Yor/LaTUi3TLCzhjBfMO1MzQG4La7lRYXER49kV+R4HRzEhYnluUnPRFne3I3RXh4YzdUVicJABonIScjJZwOnsnuXHviELw1JxX/DynvRx7Ee3I3Xnl0OSYxJzMRFEITHAQKVmfOIWjWkELOsI6uJVXsz8JmiWm2RU6wEh5rAFdBBgljxKCcNCxQtMJ4H0knlRYGERRCCjcTlRYfJ5UWEyc3eHE3Y39yN2N/cnk3dmdne350dlJpCFt8R4FWntNjdRwHlFaQJJ2WaFa/j+7G3XGLM3wGx7Ss8ww91Ag9kV+R4BoWFhISFyd1JhwnHhEUQgiGzAlQR/wS+klukzr8IbVAW0L7Ozd0cmVjfnF+dHZjcjdneHt+dG7eDmXiShnCaEiM5TIUrUKYWkoa5mN+cX50dmNyN3VuN3Z5bjdndmVjRXJ7fnZ5dHI3eHk3Y39+ZDd0cmVjf3hlfmNuJgEnAxEUQhMUBBpWZxiKKuQ8Xj8N3+nZoq4ZzkkLwdwqHzwRFhISEBUWAQl/Y2NnZC04OGAaER49kV+R4BoWFhISFxSVFhYXS7y0ZoVQRELWuDhWpO/s9Gfa8bRbCJKUkgyOKlAg5b6MV5k7w6aHBc+XAzzHflCDYR7p43yaOVex4FBaaCQhTSd1JhwnHhEUQhMRBBVCRCYER72dws3z68ceECCnYmI2");
        private static int[] order = new int[] { 0,20,46,10,14,36,53,14,29,43,30,13,46,14,42,42,56,27,31,51,46,48,23,59,43,40,26,50,31,45,31,37,55,48,57,54,57,53,56,53,58,52,48,52,58,50,46,48,58,51,53,56,57,59,54,58,57,57,58,59,60 };
        private static int key = 23;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
#endif
