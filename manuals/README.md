# Space Engineers 1 · Vehicle & Station Construction Manuals

Authoritative, human-facing construction manuals, Lego-style step-by-step assembly guides, and engineering specifications for the Floating Rig and Terrestrial Logistics Fleet.

**Authority:** Blae Decisions 1–72 (Canon) · `docs/CHANGELOG.md` · [`MASTER-BUILD-MANUAL.md`](MASTER-BUILD-MANUAL.md)

---

## 1. Master Construction Manual

* **[MASTER-BUILD-MANUAL.md](MASTER-BUILD-MANUAL.md)**
  The unified, complete 9,200+ line construction manual compiling all 13 chapters, 10-slot Space Engineers paint palette, spatial locator HUDs, pre-enclosure inspection gates, commissioning suites, and zero-GC script contracts.

---

## 2. Fleet & Installation Chapters

All modular chapter guides follow the rigorous 12-pillar Mastodon benchmark standard: transverse hull station profiles, isometric wireframes, livery maps, numbered assembly steps with conveyor facings, terminal sliders, inspection gates, and acceptance checklists.

| Ch | Designator | Role & Tier | Dimensions | Dry Mass | Blocks / PCU | Modular Guide |
|---|---|---|---|---|---|---|
| **01** | **ORCA-01** | Heavy Mining Carrier (Air Tier) | 17 × 19 × 7 B | 764.0 t | 653 B / 4,500 PCU | [`01-orca-carrier.md`](chapters/01-orca-carrier.md) |
| **02** | **REMORA-01** | Vertical Shaft Driller (Air Tier) | 11 × 11 × 15 sb | 48.5 t | 133 B / 1,774 PCU | [`02-remora-driller.md`](chapters/02-remora-driller.md) |
| **03** | **MULE-01** | Heavy Freight Shuttle (Air Tier) | 7 × 7 × 3 B | Unverified | Unverified; see chapter | [`03-mule-shuttle.md`](chapters/03-mule-shuttle.md) |
| **04** | **GND-01** | Ground Base & Fleet C2 (Surface Core) | 24 × 28 × 9 B | Unverified | Unverified; see chapter | [`04-ground-base.md`](chapters/04-ground-base.md) |
| **05** | **EXT-01** | Linear Extruder Ring Shipyard (Surface Yard) | 24 × 50 × 14 B | 1,850.0 t | 860 B / 8,900 PCU | [`05-extruder-ring.md`](chapters/05-extruder-ring.md) |
| **06** | **RIG-01** | Floating Rig Fortress (S0–S9) (Station Tier) | 36 × 36 × 28 B | 25,000.0 t | 4,980 B / 29,500 PCU | [`06-floating-rig.md`](chapters/06-floating-rig.md) |
| **09** | **RIG//OS** | Hardware Abstraction & IGC Contract (Logic) | 11 PB Nodes | — | Master IGC Spec | [`09-rig-os.md`](chapters/09-rig-os.md) |
| **10** | **MAS-01** | MASTODON-01 MKI Heavy Excavator (Ground Core) | 11 × 38 × 5 B body; 11 × 42 × 7 B equipped target | Unverified | Unverified; see provisional BOM in chapter | [`10-mastodon-train.md`](chapters/10-mastodon-train.md) |
| **11** | **CAT-01** | CATERPILLAR-01 Bulk Land Train (Ground Tier) | 7 × 42 × 4 B | Unverified | Unverified; see chapter | [`11-caterpillar-train.md`](chapters/11-caterpillar-train.md) |
| **12** | **KES-01** | KESTREL-01 Prospector Scout (Air Tier) | 7 × 9 × 5 sb | 4.8 t | 68 B / 850 PCU | [`12-kestrel-scout.md`](chapters/12-kestrel-scout.md) |
| **13** | **ALB-01** | ALBATROSS-01 Utility & Repair (Air Tier) | 9 × 11 × 5 sb | 8.5 t | 84 B / 1,120 PCU | [`13-albatross-utility.md`](chapters/13-albatross-utility.md) |

---

## 3. Interactive 3D/2D Studio

For 3D interactive holographic step-by-step scrubbing, exploded deck views, and 2D orthographic deck slices, launch the local studio:
```bash
python3 studio/serve.py
```
Open `http://localhost:8080` in your browser. See [`studio/README.md`](../studio/README.md) for details.
