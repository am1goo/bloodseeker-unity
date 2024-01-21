# Bloodseeker for Unity
Easy-as-dumb toolkit to prevent any malicious injections in your `Android` apps \
Beware of cheaters!

#### Unity Plugin
The latest version can be installed via [package manager](https://docs.unity3d.com/Manual/upm-ui-giturl.html) using following git URL: \
`https://github.com/am1goo/bloodseeker-unity.git`

#### How to use
```csharp
using BloodseekerSDK;
using BloodseekerSDK.Android;

IEnumerator Start()
{
    var op = Bloodseeker
    .Create()
    .AddTrail(new LibraryTrail("SomeLibrary")) //it will be converted to libSomeLibrary.so
    .AddTrail(new ClassNameTrail("xyz.abc.CheatActivator")) //any java class can be found here
    .AddTrail(new PackageNameTrail("xyz.abc")) //you can use package name instead of class name (but it's much slower)
    .AddTrail(new PathInApkTrail("META-INF/MANIFEST.MF")) //any files in base apk can be found here
    .SeekAsync();

    yield return op;

    var report = op.report;
    switch (report.result)
    {
        case Report.Result.NotInitialized:
            Debug.LogError("SDK not initialized");
            break;

        case Report.Result.Found:
            Debug.LogError($"Found strange code: {string.Join(";", report.evidence)}");
            break;

        case Report.Result.Ok:
            Debug.Log("Everything okay, go ahead!");
            break;
    }
}
```

#### Requirements
- Android Minimal SDK 17 (Jelly Bean MR1)

#### Tested in
- Unity 2019.4.x
- Unity 2020.3.x
