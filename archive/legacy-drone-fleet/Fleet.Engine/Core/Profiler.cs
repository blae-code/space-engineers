using System;

namespace IngameScript
{
    public partial class Program
    {
        public class Profiler
        {
            private readonly int[] _instructions;
            private readonly double[] _milliseconds;
            private readonly int _window;
            private int _writeIndex;
            private int _count;
            private int _totalInstructions;
            private double _totalMilliseconds;
            private int _peakInstructions;
            private double _peakMilliseconds;
            private int _last;

            public Profiler(int window)
            {
                if (window < 1)
                    throw new ArgumentException("Window must be >= 1", nameof(window));
                
                _window = window;
                _instructions = new int[window];
                _milliseconds = new double[window];
                _writeIndex = 0;
                _count = 0;
                _totalInstructions = 0;
                _totalMilliseconds = 0.0;
                _peakInstructions = 0;
                _peakMilliseconds = 0.0;
                _last = 0;
            }

            public void Record(int instructions, double ms)
            {
                // Remove the old value from running totals if we're overwriting an existing sample
                if (_count >= _window)
                {
                    int oldInstruction = _instructions[_writeIndex];
                    double oldMs = _milliseconds[_writeIndex];
                    
                    _totalInstructions -= oldInstruction;
                    _totalMilliseconds -= oldMs;
                }
                else
                {
                    // Increment count only when we haven't filled the window yet
                    _count++;
                }

                // Store new values
                _instructions[_writeIndex] = instructions;
                _milliseconds[_writeIndex] = ms;

                // Update running totals
                _totalInstructions += instructions;
                _totalMilliseconds += ms;

                // Update peak values if needed
                if (instructions > _peakInstructions)
                    _peakInstructions = instructions;
                
                if (ms > _peakMilliseconds)
                    _peakMilliseconds = ms;

                // Move write index to next position (circular buffer)
                _writeIndex = (_writeIndex + 1) % _window;
                
                // Update last value
                _last = instructions;
            }

            public int Last { get { return _last; } }
            
            public double Average 
            { 
                get 
                { 
                    return _count > 0 ? (double)_totalInstructions / _count : 0.0; 
                } 
            }
            
            public int Peak { get { return _peakInstructions; } }
            
            public double AverageMs 
            { 
                get 
                { 
                    return _count > 0 ? _totalMilliseconds / _count : 0.0; 
                } 
            }
            
            public double PeakMs { get { return _peakMilliseconds; } }
            
            public int Count { get { return _count; } }

            public void ResetPeak()
            {
                _peakInstructions = 0;
                _peakMilliseconds = 0.0;
            }
        }
    }
}