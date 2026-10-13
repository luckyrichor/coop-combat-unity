# 设计取舍

最后更新：2026-10-13，Codex。

## M1–M3 分层与网络
实际取舍见 [refactoring-and-network.md](refactoring-and-network.md)：纯CombatModel隔离权威规则，LocalTransport封装回环UDP，SessionView负责输入与快照呈现。每tick合并输入、序号拒重、固定步长与技能冷却由服务器处理；当前无公网可靠传输/账号认证/预测。手感验收按用户授权跳过。
