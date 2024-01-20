# bloodseeker-unity
Easy-as-dumb toolkit to prevent any malicious injections in your `Android` app \
Beware of cheaters!

#### Unity Plugin
The latest version can be installed via [package manager](https://docs.unity3d.com/Manual/upm-ui-giturl.html) using following git URL: \
`https://github.com/am1goo/bloodseeker-unity.git`

#### How to use
```csharp
IEnumerator Start()
    {
        var op = BloodseekerSDK.Bloodseeker
            .Create()
            .AddTrail(new BloodseekerSDK.Android.AndroidLibraryTrail("SomeLibrary")) //it will be converted to libSomeLibrary.so
            .AddTrail(new BloodseekerSDK.Android.AndroidClassNameTrail("xyz.abc.CheatActivator")) //any java class can be found here
            .AddTrail(new BloodseekerSDK.Android.AndroidPackageNameTrail("xyz.abc")) //you can use package name instead of class name (but it's much slower)
            .SeekAsync();

        yield return op;

        var report = op.report;
        switch (report.result)
        {
            case BloodseekerSDK.Report.Result.NotInitialized:
                Debug.LogError("SDK not initialized");
                break;

            case BloodseekerSDK.Report.Result.Found:
                Debug.LogError($"Found strange code: {string.Join(";", report.evidence)}");
                break;

            case BloodseekerSDK.Report.Result.Ok:
                Debug.Log("Everything okay, go ahead!");
                break;
        }
    }
```
