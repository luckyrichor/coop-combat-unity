# AGENTS.md

本文件为在本仓库工作的编码 agent 提供指引（Claude Code 读 `CLAUDE.md`、Codex 读 `AGENTS.md`，两者都指向这里）。

## 这是什么

Unity C# 联机战斗原型：角色控制与第三人称相机（3C）、战斗技能、简化联机同步，以及框架重构记录。

三个月求职计划六项目之一（原编号 ②），对应岗位 **09**（米哈游 Unity 游戏客户端开发 gameplay - 原神）。总计划见 [workplan-docs](https://github.com/luckyrichor/workplan-docs)。

**当前状态：未开工，环境已就绪。**

## 环境：只在 Windows 机器上开发

开发机 `luowindows`，工作区 `E:\WorkPlan`。

**Unity 装在 `F:\Unity_Hub`**（Hub 改过安装路径，别去默认的 `C:\Program Files\Unity\Hub\Editor` 找）。已装三个版本：`2022.3.38f1c1`、`6000.3.23f1`、`6000.6.0f1`。**已确定使用 `2022.3.38f1c1`**（2026-09-19 用户确认；LTS，资料最全）。建工程后再换版本代价很大，不要擅自改。

## 新建工程必须先设两项

否则 `.prefab` / `.unity` 是二进制，无法 diff 也无法合并：

- `Edit > Project Settings > Editor > Asset Serialization` → **Force Text**（配置文件里是 `m_SerializationMode: 2`）
- `Version Control Mode` → **Visible Meta Files**

参考：同机的 `E:\Unity_project\unity-learning-journal` 已经是这个设置，可以照抄。另建议把 `m_LineEndingsForNewScripts` 设为 Unix，与仓库 `.gitattributes` 的 `eol=lf` 一致。

## 分工模式：混合

- **Claude**：通过 ssh 写 C# 源码、跑 `-batchmode -runTests` 自动化测试、读日志
- **用户**：在编辑器里做场景搭建、Prefab 配置、动画状态机、Profiler 分析和肉眼验证

## Git LFS

`.fbx` / 贴图 / 音频 / `.anim` 走 LFS，规则已写在 `.gitattributes`。克隆后先 `git lfs install`。

注意 `.meta` 文件**必须跟着资产一起提交**，漏提交会导致其他机器上引用丢失。

## 所有项目共同的约束

这些是用户明确要求的，优先级高于一般工程习惯：

- **区分事实与推断**：JD 原文明确写的、与「行业常见要求／补充推断」必须显式区分。允许联网补充，但要标注来源性质。
- **不夸大**：不把规划写成已完成成果，不把本地测试数据包装成生产规模，不把加分项改写成硬性门槛。
- **不预设降级**：即使用户当下没时间反馈，也按既定方向持续推进产出，不因「时间可能不够」提前砍掉或缩水——取舍由用户自己做。
- **不安排模型算法、训练、微调、推理引擎优化方向的学习。**
- 不需要重新确认用户背景：Python 最熟练，C++/C#/Go 有基础，Agent 开发已较熟悉。直接进入框架层和有深度的工程问题，不安排语言零基础或入门教程。
- **AI 产出的代码不等于可以写进简历。** 用户要能在面试里讲清每个关键设计取舍，偏产出的项目要同步维护 `docs/design-decisions.md`。

## 多机协作

三台机器：Mac、`tx`（Ubuntu 服务器，常开）、`luowindows`（Windows，引擎唯一机器）。GitHub 是唯一事实源。

- **开工 `git pull`，收工 `git push`**，不留未推送的提交过夜
- **同一个项目同一时间只在一台机器上改**
- 密钥永不进仓库，放仓库内已 gitignore 的 `.local/`

**动环境之前先读 [`workplan-docs/环境与踩坑记录.md`](https://github.com/luckyrichor/workplan-docs/blob/main/环境与踩坑记录.md)** —— 三台的规格、三种不同的代理机制、以及十条已经踩过的坑都在那里。其中两条最容易再犯：三台文件系统大小写敏感性不一致（tx 敏感，另外两台不敏感）；PowerShell 5.1 的 BOM 规则（读 `.ps1` 必须带 BOM，写文件绝不能带）。

## 进度记录

进展写在 `docs/progress.md`：**只写已发生的事，不写计划**；失败和返工也要记，那是面试时最有料的部分。汇总到 `workplan-docs/进度总览.md`。

## Docker 运行位置（用户明确要求）

**需要 Docker 时默认跑在 `tx` 服务器上**，不要在 Mac 或 Windows 上起容器。

确有必要在本地跑时，**必须先征得用户同意**，不要自行决定。

tx 上 `ubuntu` 用户不在 `docker` 组，所有 docker 命令要带 `sudo`（免密 sudo 可用）。该机内存只有 7.5G，多个服务同时跑之前先看 `free -h`。

### 在 tx 上跑 Docker／测试前，必须先同步代码

本地改完直接去 tx 跑，而 tx 上还是旧代码 —— 结果是「测试通过」测的其实是旧版本，**这种假通过比失败更危险**。

用 `workplan-docs/scripts/run-on-tx.sh` 代替手工 ssh，它强制执行正确顺序：

```bash
bash workplan-docs/scripts/run-on-tx.sh agent-memory 'uv run pytest -q'
bash workplan-docs/scripts/run-on-tx.sh agent-memory 'docker compose up -d postgres'
```

它会：本地有未提交改动就拒绝 → push → tx pull → **校验两边哈希一致** → 才执行。哈希比对是最终判据，不是「命令有没有报错」。脚本还顺带剥掉了腾讯云登录横幅。

## 记录规范：新内容写到哪儿

**本仓库的正文只维护 `AGENTS.md` 一份**，`CLAUDE.md` 永远只是几行指路。两份都写正文必然漂移，**不要往 `CLAUDE.md` 里加任何内容**。

| 内容类型 | 去处 |
|---|---|
| 本仓库的指引、约定、边界、分工方式 | 本仓库 `AGENTS.md` |
| 环境、机器、工具链、跨平台的坑 | `workplan-docs/环境与踩坑记录.md` |
| 已经发生的进展、验证结果、失败与返工 | 本仓库 `docs/progress.md` |
| 关键设计取舍及其理由（面试要讲的） | 本仓库 `docs/design-decisions.md` |
| 排期、工时预算、里程碑 | `workplan-docs/总节奏表.md` |
| 岗位要求本身 | `workplan-docs/岗位要求原文.md`（唯一事实来源，不要在别处改写） |

写进「环境与踩坑记录」时，用 **［实测］**／**［预警］** 标注区分「已验证的事实」和「未触发的已知风险」，不要把推断写成结论。
