# 进度记录

最后更新：2026-10-02（北京时间）

本文件是 `coop-combat-unity` 的进度事实源，汇总到 `workplan-docs/进度总览.md`。

格式：每条记录写明日期、做了什么、验证方式与结果、遇到的问题。**不写计划，只写已发生的事**；失败和返工也要记，那是面试时最有料的部分。

---

## 2026-10-01 W3 维持：Windows 连接阻塞（Codex）

只读取 tx 仓库 coop-combat-unity@bc81f66（开工 clean、pull --ff-only 已同步）、AGENTS 与 3C 操作手册，没有在 tx 实现 C# 代替 Windows 开发。

tx 没有 luowindows SSH 主机配置；实际连接退出 255，名称解析失败。仓库指引没有可用 IP。无法核对 Windows 本地工程、并行工作状态、Unity 编译或执行测试；不能把 W3 维持标成完成，也不能沿用 W1 的工具链就绪结论当当前验收。

接续：恢复 tx 到 Windows 的已授权 SSH 入口，先核对 git status/编辑器进程，再进行独立小步（如 3C 控制器 EditMode 测试与边界修复）；记录实际 Unity 2022.3.38f1c1 batchmode 结果。工程未创建时必须先 Force Text/Visible Meta Files；用户场景配置与相机/角色手感肉眼验收另列。UnityGames 未提供，不推断内容。

只增加本地阻塞记录，未提交/推送。
## 2026-10-02 W5 / W7（Codex）

来源 coop-combat-unity@546868c，代码未修改。用户授权提前推进 W5–W7，并允许暂不可做项跳过。本次 pull --ff-only 无新增提交；Windows 入口 luowindows 仍无法解析，未核验远端工程/3C。

W5 独立维护与 W7 M2 两个技能的输入→判定→表现，按授权暂跳过；没有实现/编译/编辑器验收，不标完成。恢复Windows后先核实M1、Force Text与Visible Meta Files，再做两个技能与场景/Prefab/动画/肉眼验收。UnityGames仍未收到，不猜其架构。此前章节为历史记录。
