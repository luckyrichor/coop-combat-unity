# coop-combat-unity

Unity C# 联机战斗原型：角色控制与第三人称相机（3C）、战斗技能、简化联机同步，以及框架重构记录。

**状态：实际工程与M2/M3工程验收已完成；M1人工手感/观感验收依授权保留**（2026-10-13）

历史建工程手册： [`docs/操作手册-M1-工程与3C.md`](docs/操作手册-M1-工程与3C.md)——建工程、序列化设置、3C 接线的分步操作。

## 对应岗位

09 米哈游 Unity 游戏客户端开发（gameplay）- 原神。岗位原文见 [workplan-docs](https://github.com/luckyrichor/workplan-docs)。

## 环境

**只在 Windows 机器（`luowindows`）上开发**。Unity 已安装，六个项目里这条线的环境是现成的。

## 分工

- 助手通过 ssh 写 C# 源码、跑 `-batchmode -runTests` 自动化测试、读日志
- 用户在编辑器里做场景搭建、Prefab 配置、动画状态机、Profiler 分析和肉眼验证

## Unity 配置要求

为了让 `.prefab` / `.unity` / `.asset` 能进版本控制并可读 diff，必须在 Editor 里设置：

- `Edit > Project Settings > Editor > Asset Serialization` → **Force Text**
- `Version Control Mode` → **Visible Meta Files**

否则这些文件会是二进制，无法 diff 也无法合并。

## Git LFS

本仓库启用 LFS 管理 `.fbx` / 贴图 / 音频 / `.anim`。克隆后先 `git lfs install`。

## 当前可运行原型

Game/是Unity 2022.3.38f1c1实际工程。运行见[docs/run.md](docs/run.md)，已验证的模型/场景/双程序同步与人工验收边界见[重构记录](docs/refactoring-and-network.md)。
