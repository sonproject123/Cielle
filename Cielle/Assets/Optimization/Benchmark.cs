using System;
using System.Diagnostics;
using UnityEngine;

public class Benchmark : MonoBehaviour {
    [Range(0f, 1000), SerializeField] float _iterations;

    private BenchmarkTest _benchmarkTest;

    private void Awake() {
        _benchmarkTest = GetComponent<BenchmarkTest>();
    }

    [ContextMenu("RunTest")]

    public void RunTest() {
        Stopwatch sw = Stopwatch.StartNew();
        sw.Start();

        for (int i = 0; i < _iterations; i++)
            _benchmarkTest.PerformTest();

        sw.Stop();

        UnityEngine.Debug.Log(sw.ElapsedMilliseconds + "ms");
    }
}
