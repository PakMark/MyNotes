using Android.OS;
using MyNotes.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidVersion))]
namespace MyNotes.Droid
{
    public class AndroidVersion : IAndroidVersion
    {
        public bool IsHigherOreo()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O) return true;
            return false;
        }
    }
}