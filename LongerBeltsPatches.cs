using System.Collections.Generic;
using System.Reflection.Emit;
using CatAPI;
using HarmonyLib;

namespace LongerBelts
{
    [HarmonyPatch]
    public static class LongerBeltsPatches
    {
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(PlanetAuxData), "SnapLineNonAlloc")]
        private static IEnumerable<CodeInstruction> PlanetAuxData_SnapLineNonAlloc_Transpiler(
            IEnumerable<CodeInstruction> instructions,
            ILGenerator generator)
        {
            var matcher = new CodeMatcher(instructions, generator);
            matcher.MatchForward(false,
                new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(PlanetGrid), nameof(PlanetGrid.SnapLineNonAlloc))));
            matcher.SetAndAdvance(OpCodes.Call, AccessTools.Method(typeof(LongerBeltsLogic), nameof(LongerBeltsLogic.LongerSnapLineNonAlloc)));
            var snaps = matcher.InstructionAt(-3).operand;
            var jmpBeltGeneration = matcher.InstructionAt(1).operand;
            matcher.Advance(5);
            matcher.CreateLabelAt(matcher.Pos, out var jmpShorterPath);
            matcher.InsertAndAdvance(
                new CodeInstruction(OpCodes.Ldarg_3),
                new CodeInstruction(OpCodes.Ldc_I4_2),
                new CodeInstruction(OpCodes.Bne_Un_S, jmpShorterPath),
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Ldarg_2),
                new CodeInstruction(OpCodes.Ldloca_S, 0),
                new CodeInstruction(OpCodes.Ldarg_S, snaps),
                new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(LongerBeltsLogic), nameof(LongerBeltsLogic.LongerGeodesic))),
                new CodeInstruction(OpCodes.Brtrue_S, jmpBeltGeneration),
                new CodeInstruction(OpCodes.Ldloc_0),
                new CodeInstruction(OpCodes.Ret));
            return matcher.InstructionEnumeration();
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(BuildTool_Path), "DeterminePreviews")]
        private static IEnumerable<CodeInstruction> BuildTool_Path_DeterminePreviews_Transpiler(
            IEnumerable<CodeInstruction> instructions,
            ILGenerator generator)
        {
            var matcher = new CodeMatcher(instructions, generator);
            matcher.MatchForward(false,
                new CodeMatch(OpCodes.Ldarg_0),
                new CodeMatch(OpCodes.Ldc_I4_0),
                new CodeMatch(OpCodes.Stfld,
                    AccessTools.Field(typeof(BuildTool_Path), nameof(BuildTool_Path.geodesic))));
            matcher.SetAndAdvance(OpCodes.Ldarg_0, null);
            matcher.SetAndAdvance(OpCodes.Call, AccessTools.Method(typeof(LongerBeltsLogic), nameof(LongerBeltsLogic.SwitchBeltsPath)));
            matcher.SetAndAdvance(OpCodes.Nop, null);

            matcher.MatchForward(true,
                new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(PlanetAuxData), nameof(PlanetAuxData.SnapLineNonAlloc))));
            var begin = matcher.InstructionAt(-22).operand;
            var end = matcher.InstructionAt(-21).operand;
            matcher.Advance(-26)
                .InsertAndAdvance(
                    new CodeInstruction(OpCodes.Ldloc_S, begin),
                    new CodeInstruction(OpCodes.Ldloca_S, end),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(LongerBeltsLogic), nameof(LongerBeltsLogic.SlantVerticalBelts))));
            return matcher.InstructionEnumeration();
        }
    }
}
