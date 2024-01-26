# Bloodseeker for Unity
Easy-as-dumb toolkit to prevent any malicious injections in your `Android` apps \
Beware of cheaters!

## Features
- search of abnormal files, packages, libraries and even values in `AndroidManifest.xml`
- delivery of information about newest threats to users from your own server (via secured and encrypted remote update)

#### What inside
- .NET wrapper for Unity
- [bloodseeker-android](https://github.com/am1goo/bloodseeker-android) implementation as `.aar` library

#### Unity Plugin
The latest version can be installed via [package manager](https://docs.unity3d.com/Manual/upm-ui-giturl.html) using following git URL: \
`https://github.com/am1goo/bloodseeker-unity.git#0.2.0`

#### How to use
```csharp
using BloodseekerSDK;
using BloodseekerSDK.Android;

[SerializeField]
private TextAsset _localUpdateFile;

IEnumerator Start()
{
    var op = Bloodseeker
    //create instance of Bloodseeker
    .Create()
    //use local encrypted and pre-configured file with actual trails
    .SetLocalUpdateConfig(new LocalUpdateConfig
    {
        file = new TextAssetFile(_localUpdateFile),
        secretKey = "YourSecretKey",
    })
    //or download and update trails from your remote server
    .SetRemoteUpdateConfig(new RemoteUpdateConfig
    {
        //url where sdk have access to file with update
        url = "https://your.custom.domain/path/to/file.bmx",
        //secret key to decrypt (or "unlock") file
        secretKey = "YourSecretKey",
        //cache time to live (in seconds)
        cacheTTL = 60,
    })
     //it will be converted to libSomeLibrary.so
    .AddTrail(new LibraryTrail("SomeLibrary"))
    //any java class can be found here
    .AddTrail(new ClassNameTrail("xyz.abc.CheatActivator"))
    //you can use package name instead of class name (but it's much slower)
    .AddTrail(new PackageNameTrail("xyz.abc"))
    //any files in base apk can be found here
    .AddTrail(new PathInApkTrail("META-INF/MANIFEST.MF"))
    //check node "application/activity" contains "android:name" with value "com.unity3d.player.UnityPlayerActivity"
    .AddTrail(new AndroidManifestXmlTrail(AndroidManifestXmlTrail.Looker.UnityPlayerActivity()))
    //check node "application/provider" contains "android:name" with value "com.facebook.internal.FacebookInitProvider"
    .AddTrail(new AndroidManifestXmlTrail(new AndroidManifestXmlTrail.Looker(
        nodes:      new string[] { "application", "provider" },
        attribute:  "android:name",
        value:      "com.facebook.internal.FacebookInitProvider"
    )))
    //start seeking anything in these trails
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

## Requirements
- Minimal SDK 26 (Android 8.0, Oreo O)

## Plugin supports
- Perfectly works with [Beebyte Obfuscator](https://www.beebyte.co.uk/), but in some cases you should add `Bloodseeker.Runtime.dll` to array `Assemblies` in `ObfuscatorOptions.asset`

## Tested in
- Unity 2019.4.x
- Unity 2020.3.x

## Contribute
Contribution in any form is very welcome. Bugs, feature requests or feedback can be reported in form of Issues.
