# 运行与重建

Windows打开Game/（2022.3.38f1c1），打开Assets/Scenes/Combat.unity按Play。另一个客户端可用独立程序命令`CoopCombat.exe --join`加入默认127.0.0.1:45002。默认实例为host，不要在同端口启动两个host。

自动入口（PowerShell）：

```powershell
$editor='F:\Unity_Hub\2022.3.38f1c1\Editor\Unity.exe'
& $editor -batchmode -nographics -quit -projectPath "$PWD\Game" -executeMethod Setup.Build -logFile "$PWD\.local\setup.log"
& $editor -batchmode -nographics -projectPath "$PWD\Game" -runTests -testPlatform EditMode -testResults "$PWD\.local\edit-tests.xml" -logFile "$PWD\.local\edit.log"
& $editor -batchmode -nographics -projectPath "$PWD\Game" -runTests -testPlatform PlayMode -testResults "$PWD\.local\play-tests.xml" -logFile "$PWD\.local\play.log"
& $editor -batchmode -nographics -quit -projectPath "$PWD\Game" -executeMethod PlayerBuild.Run -logFile "$PWD\.local\build.log"
```

从自动脚本启动GUI编辑器时使用Start-Process -Wait -PassThru并核对日志/XML，不能只把壳层的0退出码当测试通过。NUnit XML必须实际有测试且failed=0。

`--probe --exit-seconds N`仅用于本地自动移动/快照观测，会输出逐tick结构化状态；正常玩法不启用。网络协议只回环、未加账号认证。

一键最终验收（实际已执行）：powershell -NoProfile -ExecutionPolicy Bypass -File scripts/check.ps1。它等待真实进程退出，校验NUnit非零测试、独立构建和至少50个共同双玩家tick一致；失败退出非零。可用 -SkipPlayer 仅运行规则与场景测试。
