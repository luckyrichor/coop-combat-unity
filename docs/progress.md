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


## 2026-10-07 Codex：W8–W10实际执行

本轮W8–W10：W9维护因Windows唯一开发机入口无法解析而暂跳过（实测ssh luowindows hostname，退出255）。W8/W10本项目是休息档，不编造里程碑。未运行Unity编译、batchmode、场景/Prefab或编辑器人工验收，原M1/3C/战斗技能前置缺口仍未完成。恢复Windows入口和工程后再补，此次只有进度记录更新。

## 2026-10-13 M1–M3 工程推进（Codex）

来源400862c + 本轮Windows工作树修改。继承Force Text并核对Visible Meta，生成实际场景、控制/轨道相机、冲刺与范围攻击、权威模型与UDP快照分层。EditMode5/5、PlayMode1/1、Windows独立程序构建通过；两个真实独立进程100个共同双玩家tick位置/血量/事件一致。缺失测试框架与JSON诊断未Serializable均修正并重跑；详细重构记录和证据见refactoring-and-network.md。人工手感/画面验收按授权跳过，不称全部M1主观验收完成。

## 2026-10-13 连接恢复后的最终复验（Codex）
原基线400862c及本轮Windows工作树。scripts/check.ps1完整执行：当前源码EditMode5/5、PlayMode1/1、独立构建成功、两个独立Windows程序100共同双玩家tick逐字段一致，进程退出0。source.json记录实际工程/配置/脚本SHA256；替换先前XML为此次最终同源报告。连接中断阶段的未确认状态已解除，手感/观感仍不冒充人工验收。
