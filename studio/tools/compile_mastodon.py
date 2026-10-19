#!/usr/bin/env python3
"""Compiler script that ingests docs/rig/chapters/mastodon-instructions.md
and produces the master web/data/mastodon.json dataset with all 47 steps,
placement instructions, terminal configs, parts trays, checks, and 3D blocks.
"""
import json, os, re

SRC_MD = "manuals/chapters/10-mastodon-train.md"
OUT_JSON = "studio/data/mastodon.json"

def parse_parts_tray(tray_text):
    parts = []
    lines = [l.strip() for l in tray_text.strip().split("\n") if l.strip().startswith("|")]
    if len(lines) >= 3:
        for line in lines[2:]:
            cols = [c.strip() for c in line.split("|")[1:-1]]
            if len(cols) >= 4:
                b_type = cols[0]
                qty_str = cols[1]
                slot_str = cols[2]
                mass_pcu = cols[3]
                note = cols[4] if len(cols) > 4 else ""
                try:
                    qty = int(re.search(r"\d+", qty_str).group())
                except:
                    qty = 1
                slot = 0
                slot_match = re.search(r"Slot\s+(\d+)", slot_str)
                if slot_match:
                    slot = int(slot_match.group(1))
                parts.append({
                    "type": b_type,
                    "qty": qty,
                    "slot": slot,
                    "massPcu": mass_pcu,
                    "note": note
                })
    return parts

def parse_checks(step_text):
    checks = []
    m_check = re.search(r"\*\*✓ Check:\*\*\s*(.+?)(?=\n\n|\n---|####|\Z)", step_text, re.DOTALL)
    if m_check:
        checks.append(m_check.group(1).strip().replace("\n", " "))
    gates = re.findall(r"\*\s*\[[ x]\]\s*(.+)", step_text)
    for g in gates:
        checks.append(g.strip())
    return checks

def parse_instructions(step_text):
    m = re.search(r"#### Placement Instructions\s*\n([\s\S]+?)(?=\n####|\n\*\*✓|\n---|```|\Z)", step_text)
    if m:
        lines = [l.strip() for l in m.group(1).strip().split("\n") if l.strip()]
        return lines
    return []

def parse_terminal_config(step_text):
    m = re.search(r"#### Terminal Configuration & Sliders\s*\n([\s\S]+?)(?=\n####|\n\*\*✓|\n---|```|\Z)", step_text)
    if m:
        return m.group(1).strip()
    return ""

def determine_phase(step_id):
    num = float(re.search(r"10\.(\d+)", step_id).group(1))
    if num <= 5: return "10-1"
    if num <= 14: return "10-2"
    if num <= 20: return "10-3"
    if num <= 28: return "10-4"
    if num <= 36: return "10-5"
    return "10-6"

def determine_subsystem(title, text):
    t = (title + " " + text).lower()
    if "boom" in t or "tool head" in t or "grapple" in t: return "boom"
    if "flight deck" in t or "mule" in t or "kestrel" in t or "pad" in t: return "flightdeck"
    if "bridge" in t or "cockpit" in t or "helm" in t or "console" in t or "avionics" in t or "pb" in t: return "avionics"
    if "reactor" in t or "engine" in t or "battery" in t or "power" in t or "tank h2" in t: return "power"
    if "wheel" in t or "suspension" in t or "outrigger" in t or "hitch" in t or "dpu" in t: return "propulsion"
    if "conveyor" in t or "sorter" in t or "ejector" in t or "buffer" in t or "refinery" in t or "assembler" in t: return "conveyor"
    return "structure"

def determine_level(title):
    m = re.search(r"\((L[0-9]+(?:-[0-9A-Za-z]+)?)\)", title)
    if m: return m.group(1)
    return "L2"

def generate_blocks_for_step(step_id, subsystem, level_str):
    lvl = int(re.search(r"\d+", level_str).group()) if re.search(r"\d+", level_str) else 2
    blocks = []
    B = 2.5
    
    # Precise geometric coordinate assignments for all 47 steps
    s = step_id.replace("Step 10.", "")
    if s == "1":
        for z in range(-8, 13):
            blocks.append({"pos": [-3, 2, z], "shape": "cube", "subtype": "LargeHeavyBlockArmorBlock", "paletteSlot": 0, "subsystem": "structure"})
            blocks.append({"pos": [3, 2, z], "shape": "cube", "subtype": "LargeHeavyBlockArmorBlock", "paletteSlot": 0, "subsystem": "structure"})
        for z in [9, 4, -1, 12, -8]:
            for x in range(-2, 3):
                blocks.append({"pos": [x, 2, z], "shape": "cube", "subtype": "LargeHeavyBlockArmorBlock", "paletteSlot": 0, "subsystem": "structure"})
    elif s == "2":
        for z in range(-8, 13):
            blocks.append({"pos": [-1, 1, z], "shape": "halfslope", "subtype": "HeavyArmorHalfSlope", "paletteSlot": 0, "subsystem": "structure"})
            blocks.append({"pos": [1, 1, z], "shape": "halfslope", "subtype": "HeavyArmorHalfSlope", "paletteSlot": 0, "subsystem": "structure"})
            blocks.append({"pos": [0, 1, z], "shape": "half", "subtype": "HeavyArmorHalfBlock", "paletteSlot": 0, "subsystem": "structure"})
        for x in [-2, 2]:
            blocks.append({"pos": [x, 1, 12], "shape": "slope2x1", "subtype": "HeavyArmorSlope2x1Base", "paletteSlot": 0, "subsystem": "structure"})
            blocks.append({"pos": [x, 2, 12], "shape": "slope2x1", "subtype": "HeavyArmorSlope2x1Tip", "paletteSlot": 0, "subsystem": "structure"})
    elif s == "3":
        for z in range(-8, 12):
            blocks.append({"pos": [0, 1, z], "shape": "conveyor", "subtype": "LargeConveyorTube", "paletteSlot": 4, "subsystem": "conveyor", "name": f"MAS Conveyor Z{z}"})
        blocks.append({"pos": [0, 1, 12], "shape": "conveyor", "subtype": "LargeBlockConveyor", "paletteSlot": 4, "subsystem": "conveyor", "name": "MAS Conveyor Bow Intake"})
    elif s == "4":
        blocks.append({"pos": [-4, 1, 9], "shape": "wheel5x5", "subtype": "Suspension5x5Left", "paletteSlot": 0, "subsystem": "propulsion", "name": "MAS Wheel P1"})
        blocks.append({"pos": [4, 1, 9], "shape": "wheel5x5", "subtype": "Suspension5x5Right", "paletteSlot": 0, "subsystem": "propulsion", "name": "MAS Wheel S1"})
        blocks.append({"pos": [-4, 1, 4], "shape": "wheel5x5", "subtype": "Suspension5x5Left", "paletteSlot": 0, "subsystem": "propulsion", "name": "MAS Wheel P2"})
        blocks.append({"pos": [4, 1, 4], "shape": "wheel5x5", "subtype": "Suspension5x5Right", "paletteSlot": 0, "subsystem": "propulsion", "name": "MAS Wheel S2"})
        blocks.append({"pos": [-4, 1, -1], "shape": "wheel5x5", "subtype": "Suspension5x5Left", "paletteSlot": 0, "subsystem": "propulsion", "name": "MAS Wheel P3"})
        blocks.append({"pos": [4, 1, -1], "shape": "wheel5x5", "subtype": "Suspension5x5Right", "paletteSlot": 0, "subsystem": "propulsion", "name": "MAS Wheel S3"})
    elif s == "5":
        blocks.append({"pos": [0, 1, 0], "shape": "conveyor", "subtype": "LargeConveyorSorter", "paletteSlot": 4, "subsystem": "conveyor", "name": "MAS Sorter Tailings"})
        blocks.append({"pos": [-1, 1, -1], "shape": "conveyor", "subtype": "ConveyorEjector", "paletteSlot": 4, "subsystem": "conveyor", "name": "MAS Ejector Tailings 1"})
        blocks.append({"pos": [1, 1, -1], "shape": "conveyor", "subtype": "ConveyorEjector", "paletteSlot": 4, "subsystem": "conveyor", "name": "MAS Ejector Tailings 2"})
    elif s == "6":
        blocks.append({"pos": [0, 3, 2], "shape": "cargo", "subtype": "LargeBlockLargeContainer", "paletteSlot": 5, "subsystem": "conveyor", "name": "MAS Tractor Buffer"})
    elif s == "7":
        blocks.append({"pos": [0, 3, -2], "shape": "reactor", "subtype": "LargeBlockSmallGenerator", "paletteSlot": 6, "subsystem": "power", "name": "MAS Reactor 1"})
        blocks.append({"pos": [0, 3, -3], "shape": "reactor", "subtype": "LargeBlockSmallGenerator", "paletteSlot": 6, "subsystem": "power", "name": "MAS Reactor 2"})
    elif s == "8":
        blocks.append({"pos": [0, 3, -4], "shape": "engine", "subtype": "LargeHydrogenEngine", "paletteSlot": 6, "subsystem": "power", "name": "MAS Engine H2"})
    elif s == "9":
        for z in range(-7, 7):
            blocks.append({"pos": [-2, 3, z], "shape": "catwalk", "subtype": "CatwalkBlock", "paletteSlot": 9, "subsystem": "structure", "name": f"Jefferies Grate Z{z}"})
    elif s == "10":
        blocks.append({"pos": [0, 2, -8], "shape": "rotor", "subtype": "LargeAdvancedRotor", "paletteSlot": 2, "subsystem": "propulsion", "name": "MAS Hitch Azimuth"})
        blocks.append({"pos": [0, 3, -8], "shape": "hinge", "subtype": "LargeHinge", "paletteSlot": 2, "subsystem": "propulsion", "name": "MAS Hitch Pitch"})
        blocks.append({"pos": [0, 3, -7], "shape": "cube", "subtype": "LargeMergeBlock", "paletteSlot": 2, "subsystem": "structure", "name": "MAS Hitch Sever Tongue"})
    elif s == "11":
        blocks.append({"pos": [-1, 3, -8], "shape": "cube", "subtype": "LargeMagneticPlate", "paletteSlot": 2, "subsystem": "propulsion", "name": "MAS Hitch Clamp Port"})
        blocks.append({"pos": [1, 3, -8], "shape": "cube", "subtype": "LargeMagneticPlate", "paletteSlot": 2, "subsystem": "propulsion", "name": "MAS Hitch Clamp Stbd"})
    elif s == "12":
        blocks.append({"pos": [-3, 2, 2], "shape": "cube", "subtype": "LargeBlockLocker", "paletteSlot": 5, "subsystem": "structure", "name": "MAS Locker Port"})
        blocks.append({"pos": [3, 2, 2], "shape": "cube", "subtype": "LargeBlockLocker", "paletteSlot": 5, "subsystem": "structure", "name": "MAS Locker Stbd"})
    elif s == "13":
        blocks.append({"pos": [-3, 1, 0], "shape": "cube", "subtype": "DecoyBlock", "paletteSlot": 0, "subsystem": "avionics", "name": "MAS Decoy Flank Port"})
        blocks.append({"pos": [3, 1, 0], "shape": "cube", "subtype": "DecoyBlock", "paletteSlot": 0, "subsystem": "avionics", "name": "MAS Decoy Flank Stbd"})
        blocks.append({"pos": [0, 1, 6], "shape": "cube", "subtype": "GyroBlock", "paletteSlot": 0, "subsystem": "avionics", "name": "MAS Gyro Recovery 1"})
    elif s == "14":
        blocks.append({"pos": [-5, 1, 11], "shape": "piston", "subtype": "PistonOutrigger", "paletteSlot": 1, "subsystem": "propulsion", "name": "MAS Outrigger P-Fwd"})
        blocks.append({"pos": [5, 1, 11], "shape": "piston", "subtype": "PistonOutrigger", "paletteSlot": 1, "subsystem": "propulsion", "name": "MAS Outrigger S-Fwd"})
        blocks.append({"pos": [-5, 1, -6], "shape": "piston", "subtype": "PistonOutrigger", "paletteSlot": 1, "subsystem": "propulsion", "name": "MAS Outrigger P-Aft"})
        blocks.append({"pos": [5, 1, -6], "shape": "piston", "subtype": "PistonOutrigger", "paletteSlot": 1, "subsystem": "propulsion", "name": "MAS Outrigger S-Aft"})
    elif s == "15":
        blocks.append({"pos": [0, 4, 6], "shape": "rotor", "subtype": "LargeAdvancedRotor", "paletteSlot": 3, "subsystem": "boom", "name": "MAS Turret Azimuth"})
    elif s == "16":
        blocks.append({"pos": [0, 5, 6], "shape": "hinge", "subtype": "LargeHinge", "paletteSlot": 3, "subsystem": "boom", "name": "MAS Boom Shoulder"})
        for z in range(3, 6):
            blocks.append({"pos": [0, 5, z], "shape": "cube", "subtype": "LargeBlockArmorBlock", "paletteSlot": 3, "subsystem": "boom", "name": f"Main Boom Segment Z{z}"})
    elif s == "17":
        blocks.append({"pos": [0, 5, -2], "shape": "hinge", "subtype": "LargeHinge", "paletteSlot": 3, "subsystem": "boom", "name": "MAS Boom Elbow"})
        for z in range(-5, -2):
            blocks.append({"pos": [0, 5, z], "shape": "cube", "subtype": "LargeBlockArmorBlock", "paletteSlot": 3, "subsystem": "boom", "name": f"Forearm Girder Z{z}"})
    elif s == "18":
        blocks.append({"pos": [0, 5, -6], "shape": "rotor", "subtype": "LargeAdvancedRotor", "paletteSlot": 3, "subsystem": "boom", "name": "MAS Wrist Roll"})
        blocks.append({"pos": [0, 5, -7], "shape": "hinge", "subtype": "LargeHinge", "paletteSlot": 3, "subsystem": "boom", "name": "MAS Wrist Pitch"})
    elif s == "19":
        blocks.append({"pos": [-1, 4, -7], "shape": "turret", "subtype": "SimpleLaserMultitool", "paletteSlot": 3, "subsystem": "boom", "name": "MAS Laser 1"})
        blocks.append({"pos": [1, 4, -7], "shape": "turret", "subtype": "SimpleLaserMultitool", "paletteSlot": 3, "subsystem": "boom", "name": "MAS Laser 2"})
        blocks.append({"pos": [0, 4, -7], "shape": "cube", "subtype": "LargeMagneticPlate", "paletteSlot": 9, "subsystem": "boom", "name": "MAS Tool Grapple"})
        blocks.append({"pos": [0, 4, -8], "shape": "cube", "subtype": "LargeMagneticPlate", "paletteSlot": 1, "subsystem": "structure", "name": "MAS Clamp Boom Stow"})
    elif s == "20":
        blocks.append({"pos": [0, 4, 7], "shape": "cube", "subtype": "ProgrammableBlock", "paletteSlot": 7, "subsystem": "avionics", "name": "[MAS] PB Kinematics Macro Core"})
    elif s == "21":
        for x in range(-3, 4):
            for z in range(7, 12):
                blocks.append({"pos": [x, 3, z], "shape": "cube", "subtype": "LargeHeavyBlockArmorBlock", "paletteSlot": 0, "subsystem": "structure"})
        blocks.append({"pos": [-3, 3, 8], "shape": "cube", "subtype": "AirlockDoorBlock", "paletteSlot": 7, "subsystem": "structure", "name": "MAS Door Airlock Outer"})
    elif s == "22":
        blocks.append({"pos": [0, 4, 9], "shape": "cockpit", "subtype": "CockpitOpen", "paletteSlot": 7, "subsystem": "avionics", "name": "MAS Helm"})
        blocks.append({"pos": [-2, 4, 9], "shape": "cockpit", "subtype": "CockpitOpen", "paletteSlot": 7, "subsystem": "avionics", "name": "MAS Station Boom"})
        blocks.append({"pos": [2, 4, 9], "shape": "cockpit", "subtype": "CockpitOpen", "paletteSlot": 7, "subsystem": "avionics", "name": "MAS Station Fleet"})
    elif s == "23":
        for x in range(-3, 4):
            blocks.append({"pos": [x, 4, 11], "shape": "slope2x1", "subtype": "HeavyArmorGlassSloped", "paletteSlot": 7, "subsystem": "structure", "name": "Glacis Canopy"})
            blocks.append({"pos": [x, 5, 10], "shape": "slope2x1", "subtype": "HeavyArmorSlope2x1Base", "paletteSlot": 7, "subsystem": "structure", "name": "Cab Visor"})
    elif s == "24":
        blocks.append({"pos": [-1, 4, 7], "shape": "cube", "subtype": "ProgrammableBlock", "paletteSlot": 7, "subsystem": "avionics", "name": "[MAS] PB Excavator"})
        blocks.append({"pos": [1, 4, 7], "shape": "cube", "subtype": "ProgrammableBlock", "paletteSlot": 7, "subsystem": "avionics", "name": "[MAS] PB RIG//OS-Lite"})
        blocks.append({"pos": [0, 6, 7], "shape": "cube", "subtype": "LaserAntenna", "paletteSlot": 7, "subsystem": "avionics", "name": "MAS Laser Antenna 40km"})
    elif s == "25":
        blocks.append({"pos": [0, 3, 7], "shape": "cube", "subtype": "WarheadBlock", "paletteSlot": 6, "subsystem": "avionics", "name": "MAS Warhead Scuttle Bridge"})
        blocks.append({"pos": [0, 3, -1], "shape": "cube", "subtype": "WarheadBlock", "paletteSlot": 6, "subsystem": "power", "name": "MAS Warhead Scuttle Power"})
    elif s == "26":
        blocks.append({"pos": [-2, 4, 8], "shape": "cube", "subtype": "ButtonPanelSciFi", "paletteSlot": 7, "subsystem": "avionics", "name": "MAS BP Bridge Airlock"})
        blocks.append({"pos": [2, 4, 8], "shape": "cube", "subtype": "ButtonPanelSciFi", "paletteSlot": 7, "subsystem": "avionics", "name": "MAS BP Bridge LifeSupport"})
    elif s == "27":
        blocks.append({"pos": [-4, 2, 8], "shape": "hinge", "subtype": "LargeHinge", "paletteSlot": 9, "subsystem": "structure", "name": "MAS Hinge Boarding Ladder"})
    elif s == "28":
        blocks.append({"pos": [0, 5, 9], "shape": "turret", "subtype": "LargeGatlingTurret", "paletteSlot": 7, "subsystem": "avionics", "name": "MAS Turret Bow"})
        blocks.append({"pos": [0, 5, 8], "shape": "turret", "subtype": "SimpleLaserMultitoolTurret", "paletteSlot": 3, "subsystem": "avionics", "name": "MAS MT Turret Bow"})
    elif s == "29":
        for z in range(-26, -8):
            blocks.append({"pos": [-3, 2, z], "shape": "cube", "subtype": "LargeHeavyBlockArmorBlock", "paletteSlot": 0, "subsystem": "structure"})
            blocks.append({"pos": [3, 2, z], "shape": "cube", "subtype": "LargeHeavyBlockArmorBlock", "paletteSlot": 0, "subsystem": "structure"})
        for z in [-26, -22, -14, -9]:
            for x in range(-2, 3):
                blocks.append({"pos": [x, 2, z], "shape": "cube", "subtype": "LargeHeavyBlockArmorBlock", "paletteSlot": 0, "subsystem": "structure"})
    elif s == "30":
        blocks.append({"pos": [-4, 1, -14], "shape": "wheel5x5", "subtype": "Suspension5x5Left", "paletteSlot": 0, "subsystem": "propulsion", "name": "MAS Wheel P4"})
        blocks.append({"pos": [4, 1, -14], "shape": "wheel5x5", "subtype": "Suspension5x5Right", "paletteSlot": 0, "subsystem": "propulsion", "name": "MAS Wheel S4"})
        blocks.append({"pos": [-4, 1, -22], "shape": "wheel5x5", "subtype": "Suspension5x5Left", "paletteSlot": 0, "subsystem": "propulsion", "name": "MAS Wheel P5"})
        blocks.append({"pos": [4, 1, -22], "shape": "wheel5x5", "subtype": "Suspension5x5Right", "paletteSlot": 0, "subsystem": "propulsion", "name": "MAS Wheel S5"})
    elif s == "31":
        blocks.append({"pos": [-1, 2, -17], "shape": "battery", "subtype": "LargeBlockBatteryBlock", "paletteSlot": 6, "subsystem": "power", "name": "MAS Battery Utility 1"})
        blocks.append({"pos": [1, 2, -17], "shape": "battery", "subtype": "LargeBlockBatteryBlock", "paletteSlot": 6, "subsystem": "power", "name": "MAS Battery Utility 2"})
        blocks.append({"pos": [0, 2, -18], "shape": "battery", "subtype": "LargeBlockBatteryBlock", "paletteSlot": 6, "subsystem": "power", "name": "MAS Battery Utility 3"})
        blocks.append({"pos": [0, 2, -15], "shape": "cargo_small", "subtype": "LargeBlockSmallContainer", "paletteSlot": 2, "subsystem": "power", "name": "MAS Hold Munitions"})
    elif s == "32":
        blocks.append({"pos": [-2, 3, -14], "shape": "assembler", "subtype": "LargeAssembler", "paletteSlot": 5, "subsystem": "conveyor", "name": "MAS Utility Assembler"})
        blocks.append({"pos": [-2, 3, -13], "shape": "cube", "subtype": "LargeBlockLocker", "paletteSlot": 7, "subsystem": "avionics", "name": "MAS Armory Workshop"})
        blocks.append({"pos": [2, 3, -13], "shape": "cube", "subtype": "MedicalRoom", "paletteSlot": 5, "subsystem": "avionics", "name": "MAS Medical Bay"})
        blocks.append({"pos": [2, 3, -14], "shape": "cube", "subtype": "CryoChamber", "paletteSlot": 5, "subsystem": "avionics", "name": "MAS Cryo Pod 1"})
        blocks.append({"pos": [2, 3, -15], "shape": "cube", "subtype": "CryoChamber", "paletteSlot": 5, "subsystem": "avionics", "name": "MAS Cryo Pod 2"})
    elif s == "33":
        blocks.append({"pos": [-2, 3, -12], "shape": "refinery", "subtype": "BasicRefinery", "paletteSlot": 4, "subsystem": "conveyor", "name": "MAS Field Refinery"})
        blocks.append({"pos": [0, 3, -9], "shape": "conveyor", "subtype": "LargeConveyorSorter", "paletteSlot": 4, "subsystem": "conveyor", "name": "MAS Sorter Ammo Fwd"})
    elif s == "34":
        blocks.append({"pos": [0, 3, -9], "shape": "cube", "subtype": "AirlockDoorBlock", "paletteSlot": 2, "subsystem": "structure", "name": "MAS Door Gangway Fwd"})
        blocks.append({"pos": [0, 3, -10], "shape": "cube", "subtype": "AirlockDoorBlock", "paletteSlot": 2, "subsystem": "structure", "name": "MAS Door Gangway Aft"})
    elif s == "35":
        blocks.append({"pos": [-4, 4, -13], "shape": "cube", "subtype": "AirVentBlock", "paletteSlot": 0, "subsystem": "avionics", "name": "MAS Vent Atmo Intake Port"})
        blocks.append({"pos": [4, 4, -13], "shape": "cube", "subtype": "AirVentBlock", "paletteSlot": 0, "subsystem": "avionics", "name": "MAS Vent Atmo Intake Stbd"})
    elif s == "35B":
        blocks.append({"pos": [0, 6, 6], "shape": "cube", "subtype": "DecoyBlock", "paletteSlot": 0, "subsystem": "avionics", "name": "MAS Decoy Mast"})
    elif s == "36":
        blocks.append({"pos": [0, 4, -22], "shape": "cargo", "subtype": "LargeBlockLargeContainer", "paletteSlot": 5, "subsystem": "conveyor", "name": "MAS Hold Components"})
        blocks.append({"pos": [0, 3, -11], "shape": "cube", "subtype": "ProgrammableBlock", "paletteSlot": 7, "subsystem": "avionics", "name": "[MAS-U] PB Utility"})
    elif s == "36B":
        blocks.append({"pos": [-3, 4, -10], "shape": "turret", "subtype": "SimpleLaserMultitoolTurret", "paletteSlot": 3, "subsystem": "avionics", "name": "MAS MT Turret Port"})
        blocks.append({"pos": [3, 4, -10], "shape": "turret", "subtype": "SimpleLaserMultitoolTurret", "paletteSlot": 3, "subsystem": "avionics", "name": "MAS MT Turret Stbd"})
    elif s == "37":
        for x in range(-3, 4):
            for z in range(-25, -18):
                blocks.append({"pos": [x, 6, z], "shape": "cube", "subtype": "LargeHeavyBlockArmorBlock", "paletteSlot": 1, "subsystem": "flightdeck", "name": "Mule Cradle Plate"})
        blocks.append({"pos": [0, 6, -22], "shape": "conveyor", "subtype": "LargeBlockConnector", "paletteSlot": 4, "subsystem": "conveyor", "name": "MAS MP Conn"})
        blocks.append({"pos": [0, 5, -22], "shape": "conveyor", "subtype": "LargeConveyorSorter", "paletteSlot": 4, "subsystem": "conveyor", "name": "MAS MP Sorter"})
        blocks.append({"pos": [-3, 6, -22], "shape": "hinge", "subtype": "LargeHinge", "paletteSlot": 1, "subsystem": "flightdeck", "name": "MAS MP Clamp Hinge Port"})
        blocks.append({"pos": [3, 6, -22], "shape": "hinge", "subtype": "LargeHinge", "paletteSlot": 1, "subsystem": "flightdeck", "name": "MAS MP Clamp Hinge Stbd"})
    elif s == "38":
        blocks.append({"pos": [-4, 6, -24], "shape": "solar", "subtype": "SolarPanel", "paletteSlot": 0, "subsystem": "power", "name": "MAS Solar 1"})
        blocks.append({"pos": [-4, 6, -20], "shape": "solar", "subtype": "SolarPanel", "paletteSlot": 0, "subsystem": "power", "name": "MAS Solar 2"})
        blocks.append({"pos": [4, 6, -24], "shape": "solar", "subtype": "SolarPanel", "paletteSlot": 0, "subsystem": "power", "name": "MAS Solar 3"})
        blocks.append({"pos": [4, 6, -20], "shape": "solar", "subtype": "SolarPanel", "paletteSlot": 0, "subsystem": "power", "name": "MAS Solar 4"})
    elif s == "39":
        for z in range(-25, -9):
            blocks.append({"pos": [-4, 6, z], "shape": "catwalk", "subtype": "CatwalkBlock", "paletteSlot": 9, "subsystem": "flightdeck", "name": "Catwalk Port"})
            blocks.append({"pos": [4, 6, z], "shape": "catwalk", "subtype": "CatwalkBlock", "paletteSlot": 9, "subsystem": "flightdeck", "name": "Catwalk Stbd"})
    elif s == "40":
        blocks.append({"pos": [-3, 5, -12], "shape": "conveyor", "subtype": "LargeBlockConnector", "paletteSlot": 8, "subsystem": "flightdeck", "name": "MAS Kestrel Dock 1"})
        blocks.append({"pos": [3, 5, -12], "shape": "conveyor", "subtype": "LargeBlockConnector", "paletteSlot": 8, "subsystem": "flightdeck", "name": "MAS Kestrel Dock 2"})
    elif s == "41":
        blocks.append({"pos": [-3, 4, -12], "shape": "welder", "subtype": "LargeShipWelder", "paletteSlot": 4, "subsystem": "flightdeck", "name": "MAS Welder Pad Kestrel 1"})
        blocks.append({"pos": [3, 4, -12], "shape": "welder", "subtype": "LargeShipWelder", "paletteSlot": 4, "subsystem": "flightdeck", "name": "MAS Welder Pad Kestrel 2"})
    elif s == "42":
        for x, z in [(-3, -10), (3, -10), (-3, -24), (3, -24)]:
            blocks.append({"pos": [x, 6, z], "shape": "cube", "subtype": "SpotlightBlock", "paletteSlot": 1, "subsystem": "avionics", "name": f"MAS Floodlight {x}_{z}"})
    elif s == "43":
        blocks.append({"pos": [0, 2, -26], "shape": "conveyor", "subtype": "LargeBlockConnector", "paletteSlot": 2, "subsystem": "conveyor", "name": "MAS Stern Dump Conn"})
        blocks.append({"pos": [0, 2, -25], "shape": "piston", "subtype": "PistonLinear", "paletteSlot": 2, "subsystem": "propulsion", "name": "MAS Piston Stern Dock"})
        blocks.append({"pos": [0, 2, -24], "shape": "conveyor", "subtype": "LargeConveyorSorter", "paletteSlot": 4, "subsystem": "conveyor", "name": "MAS Stern Sorter"})
        blocks.append({"pos": [-3, 1, -25], "shape": "piston", "subtype": "PistonOutrigger", "paletteSlot": 1, "subsystem": "propulsion", "name": "MAS Outrigger Utility Port"})
        blocks.append({"pos": [3, 1, -25], "shape": "piston", "subtype": "PistonOutrigger", "paletteSlot": 1, "subsystem": "propulsion", "name": "MAS Outrigger Utility Stbd"})
    elif s == "44":
        blocks.append({"pos": [0, 5, -26], "shape": "turret", "subtype": "LargeGatlingTurret", "paletteSlot": 7, "subsystem": "avionics", "name": "MAS Turret Stern"})
        blocks.append({"pos": [0, 4, -26], "shape": "conveyor", "subtype": "LargeBlockConveyor", "paletteSlot": 4, "subsystem": "conveyor", "name": "MAS Conveyor Stern Turret"})
    elif s == "44B":
        blocks.append({"pos": [0, 7, -23], "shape": "turret", "subtype": "SimpleLaserMultitoolTurret", "paletteSlot": 3, "subsystem": "avionics", "name": "MAS MT Turret Stern"})
        blocks.append({"pos": [0, 5, -20], "shape": "cargo_small", "subtype": "LargeBlockSmallContainer", "paletteSlot": 5, "subsystem": "conveyor", "name": "MAS Hold Resupply Export"})
    else:
        # Default placeholder block if any
        blocks.append({"pos": [0, lvl, 0], "shape": "cube", "subtype": "LargeHeavyBlockArmorBlock", "paletteSlot": 0, "subsystem": subsystem})

    return blocks

def main():
    with open(SRC_MD, "r", encoding="utf-8") as f:
        content = f.read()

    # Split by step headers
    step_splits = re.split(r"\n### (Step 10\.[0-9A-Za-z]+) · ([^\n]+)\n", content)
    
    steps = []
    # step_splits[0] is preamble; then groups of (step_id, title, body)
    for i in range(1, len(step_splits), 3):
        step_id = step_splits[i]
        title = step_splits[i+1].strip()
        body = step_splits[i+2]
        
        parts_tray_match = re.search(r"#### Step 10\.[0-9A-Za-z]+ Parts Tray\s*\n([\s\S]+?)(?=\n```|\n#### Placement|\n---|###|\Z)", body)
        parts = parse_parts_tray(parts_tray_match.group(1)) if parts_tray_match else []
        instructions = parse_instructions(body)
        term_config = parse_terminal_config(body)
        checks = parse_checks(body)
        
        phase = determine_phase(step_id)
        subsystem = determine_subsystem(title, body)
        level = determine_level(title)
        blocks = generate_blocks_for_step(step_id, subsystem, level)

        steps.append({
            "step": step_id.replace("Step ", ""),
            "phase": phase,
            "title": title,
            "level": level,
            "subsystem": subsystem,
            "partsTray": parts,
            "instructions": instructions,
            "terminalConfig": term_config,
            "checks": checks,
            "blocks": blocks
        })

    master_data = {
        "$schema": "https://json-schema.org/draft/2020-12/schema",
        "id": "MAS-01",
        "name": "MASTODON-01 MKI Expeditionary Excavator & Mobile FOB",
        "chapter": 10,
        "gridSize": "Large",
        "blockMeters": 2.5,
        "stats": {
            "totalBlocks": 1126,
            "dryMassTonnes": 1151.8,
            "pcu": 10015,
            "dimensionsMeters": [27.5, 105.0, 17.5],
            "dimensionsBlocks": [11, 42, 7],
            "operatingRadius": "35-45 m",
            "powerBaseloadMW": 30.0,
            "powerPeakMW": 72.0,
            "batteryCapacityMWh": 18.0,
            "orePayloadTonnes": 2500.0
        },
        "bounds": { "min": [-5, 1, -26], "max": [5, 7, 12] },
        "palette": [
            { "slot": 0, "name": "Industrial Dark Gray", "hex": "#2B303A", "rgb": [43, 48, 58], "role": "Lower chassis box girders, wheel arch frames, belly skid plates" },
            { "slot": 1, "name": "Hazard Amber", "hex": "#E6A100", "rgb": [230, 161, 0], "role": "Roof Mule Gantry Pad (7x7 B), cradle arms, outrigger warning chevrons" },
            { "slot": 2, "name": "Heavy Rail Crimson", "hex": "#9E2A2B", "rgb": [158, 42, 43], "role": "Stern train dump bumper, rear alignment push-blocks, drawbar hitches" },
            { "slot": 3, "name": "Excavator Ochre", "hex": "#E67E22", "rgb": [230, 126, 34], "role": "45 m Long-Reach Articulated Boom, arm casings, laser tool head" },
            { "slot": 4, "name": "Processing Teal", "hex": "#1ABC9C", "rgb": [26, 188, 156], "role": "Central Ore Conveyor Spine, bow intake throat, manifold junctions" },
            { "slot": 5, "name": "Logistics Emerald", "hex": "#27AE60", "rgb": [39, 174, 96], "role": "Tractor ore buffer and Utility component storage holds" },
            { "slot": 6, "name": "High-Voltage Alert Red", "hex": "#C0392B", "rgb": [192, 57, 43], "role": "Tractor Power Vault (2x Small Nuclear Reactors, 2x Large Batteries)" },
            { "slot": 7, "name": "Fleet Command Navy", "hex": "#1B263B", "rgb": [27, 38, 59], "role": "Forward Armored C2 Bridge exterior shell, operator consoles" },
            { "slot": 8, "name": "Scout Flight White", "hex": "#ECF0F1", "rgb": [236, 240, 241], "role": "Forward twin Kestrel scout drone landing pads (P1, S1) on Utility roof" },
            { "slot": 9, "name": "High-Viz Safety Yellow", "hex": "#F1C40F", "rgb": [241, 196, 15], "role": "Exterior catwalks, maintenance ladders, corner outrigger foot pads" }
        ],
        "phases": [
            { "phase": "10-1", "title": "Tractor Belly, Powertrain, Axles 1-3 & Bedrock Jacks", "envelope": "11 W × 20 L × 2 H B", "levels": ["L1", "L2"], "gate": "10.1A" },
            { "phase": "10-2", "title": "Tractor Power Core, Ore Buffer & 2-Axis Drawbar Hitch", "envelope": "7 W × 14 L × 3 H B", "levels": ["L2", "L3", "L4"], "gate": "10.2A" },
            { "phase": "10-3", "title": "Tractor Armored Bridge, 3-Seat C2 & Panoramic Canopy", "envelope": "7 W × 7 L × 3 H B", "levels": ["L3", "L4", "L5", "L6"], "gate": "10.3A" },
            { "phase": "10-4", "title": "Carriage 2 Chassis, Powertrain & Utility Sub-Floor", "envelope": "11 W × 18 L × 2 H B", "levels": ["L1", "L2"], "gate": "10.4A" },
            { "phase": "10-5", "title": "Carriage 2 Field Workshop & Closed-Loop Metallurgy Core", "envelope": "9 W × 16 L × 3 H B", "levels": ["L3", "L4", "L5"], "gate": "10.5A" },
            { "phase": "10-6", "title": "Carriage 2 Flight Decks, 45 m Boom & Final Commissioning", "envelope": "11 W × 42 L × 7 H B", "levels": ["L5", "L6", "L7"], "gate": "10.6A" }
        ],
        "steps": steps
    }

    with open(OUT_JSON, "w", encoding="utf-8") as f:
        json.dump(master_data, f, indent=2)

    total_blocks = sum(len(s["blocks"]) for s in steps)
    print(f"Successfully compiled {len(steps)} steps and {total_blocks} block definitions into {OUT_JSON}.")

if __name__ == "__main__":
    main()
