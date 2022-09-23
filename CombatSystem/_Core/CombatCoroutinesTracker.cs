using System.Collections.Generic;
using MEC;

namespace CombatSystem._Core
{
    public sealed class CombatCoroutinesTracker
    {
        public const int CombatMainLayer = 4;

        public static CoroutineHandle StartCombatCoroutine(IEnumerator<float> _coroutine, Segment segment = Segment.RealtimeUpdate)
        {
            return Timing.RunCoroutine(_coroutine, segment, CombatMainLayer);
        }

        public static CoroutineHandle StartCombatCoroutineAsMain(IEnumerator<float> _coroutine)
        {
            var tempoHandle = Timing.RunCoroutine(_coroutine, Segment.RealtimeUpdate, CombatMainLayer);
            Timing.RunCoroutine(_TrackForTempoIsAlive(), Segment.EndOfFrame, CombatMainLayer);
            return tempoHandle;

            IEnumerator<float> _TrackForTempoIsAlive()
            {
                yield return Timing.WaitUntilDone(tempoHandle);
                KillCombatCoroutines();
            }
        }

        public static void KillCombatCoroutines()
        {
            Timing.KillCoroutines(CombatMainLayer);
        }
    }
}
