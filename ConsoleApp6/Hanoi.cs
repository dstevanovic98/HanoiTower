namespace ConsoleApp6
{

    public enum HanoiType
    {
        K13_01,
        K13_12,
        K13e_01,
        K13e_12,
        K13e_23,
        K13e_30,
        P4_01,
        P4_12,
        P4_23,
        P4_31,
        C4_01,
        C4_12,
        K4e_01,
        K4e_12,
        K4e_23,
    }


    public interface IHanoi
    {
       HanoiType type { get; set; }
       byte numDiscs { get; set; }
    //   byte numPegs { get; set; }
    }
    



    public abstract class Hanoi : IHanoi
    {
        public byte numDiscs { get; set; }

        public readonly byte numPegs = 4;
        public HanoiType type { get; set; }

       // Iniate data types for all 
        public HashSet<long> setPrev = new HashSet<long>();
        public HashSet<long> setCurrent = new HashSet<long>();
        public HashSet<long> setNew = new HashSet<long>();
        public byte[] stateArray;
        public bool[] canMoveArray;


        public byte[] newState;
        public long currentState;
        public short currentDistance;
        public uint finalState = 0;

        //Print Hanoi types
        public static HanoiType SelectHanoiType()
        {
            Console.WriteLine(">> Select coloring type:");
            WriteHanoiTypes();
#pragma warning disable CS8604 // Possible null reference argument.
            return (HanoiType)Enum.Parse(typeof(HanoiType), Console.ReadLine());
#pragma warning restore CS8604 // Possible null reference argument.
        }
        private static void WriteHanoiTypes()
        {
            foreach (string s in Enum.GetNames(typeof(HanoiType)))
            {
                Console.WriteLine("\t" + (int)Enum.Parse(typeof(HanoiType), s) + " - " + s);
            }
        }


        public uint StateToLong(byte[] state)
        {
            uint num = 0;
            uint factor = 1;
            for (int i = state.Length - 1; i >= 0; i--)
            {
                num += state[i] * factor;
                factor *= this.numPegs;
            }
            return num;
        } //predstavimo byte data v številčno uint. The StateToLong method is a function that converts a state represented as a byte array into a single unsig.integer.
        public byte[] LongToState(long num)
        {
            byte[] tmpState = new byte[this.numDiscs];
            for (int i = numDiscs - 1; i >= 0; i--)
            {
                tmpState[i] = (byte)(num % this.numPegs);
                num /= this.numPegs;
            }
            return tmpState;
        } //sp


        public byte[] ArrayAllEqual(byte pegNumber)
        {
            byte[] arr = new byte[numDiscs];
            for (byte i = 0; i < arr.Length; i++)
                arr[i] = pegNumber;
            return arr;
        }
        public void ResetArray(bool[] array)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = true;
        }


        public void InitIgnoredStates(HanoiType type)
        {
            switch (type)
            {
                case HanoiType.K13_01:
                    AddStateLeading3();
                    AddStateLeading1Then3();
                    break;
            }
        }
        public void AddStateLeading1Then3()
        {
            byte[] newState;
            for (int i = 1; i < numDiscs; i++)
            {
                newState = new byte[numDiscs];
                newState[0] = 1;
                for (int j = 1; j <= i; j++)
                    newState[j] = 3;
            }
        }
        public void AddStateLeading3()
        {
            byte[] newState;
            for (int i = 0; i < numDiscs; i++)
            {
                newState = new byte[numDiscs];
                for (int j = 0; j <= i; j++)
                    newState[j] = 3;
            }
        }
        public void AddNewState(byte[] state, int disc, byte toPeg)
        {
            newState = new byte[state.Length];
            for (int x = 0; x < state.Length; x++)
                newState[x] = state[x];
            newState[disc] = toPeg;
            currentState = StateToLong(newState);
            if (!setPrev.Contains(currentState))
            {
                setNew.Add(currentState);
            }
        }
        public uint StateAllEqual(byte pegNumber)
        {
            uint num = 0;
            uint factor = 1;
            {
                num += pegNumber * factor;
                factor *= this.numPegs;
            }
            return num;
        }//sp
        public uint FinalState()
        {
            uint num = 0;
            uint factor = 1;
            {
                num += factor;
                factor *= this.numPegs;
            }
            return num;
        }

        public Hanoi() { }
    }
        public class K13 : Hanoi 
        {
           
            private void MakeMoveForSmallDimension_K13(byte[] state)
            {
                ResetArray(canMoveArray);

                for (int i = 0; i < numDiscs; i++)
                {
                    if (canMoveArray[state[i]])
                    {
                        if (state[i] == 0)
                        {
                            for (byte j = 1; j < numPegs; j++)
                            {
                                if (canMoveArray[j])
                                {
                                    AddNewState(state, i, j);
                                }
                            }
                        }
                        else // From other vertices we can only move to center
                        {
                            if (canMoveArray[0])
                            {
                                AddNewState(state, i, 0);
                            }
                        }
                    }
                    canMoveArray[state[i]] = false;
                }
            }

            private void MakeMoveForSmallDimension_K13_01_Fast(byte[] state)
            {
                ResetArray(canMoveArray);

                for (int i = 0; i < numDiscs - 2; i++)
                {
                    if (canMoveArray[state[i]])
                    {
                        if (state[i] == 0)
                        {
                            for (byte j = 1; j < numPegs; j++)
                            {
                                if (canMoveArray[j])
                                {
                                    AddNewState(state, i, j);
                                }
                            }
                        }
                        else // From other vertices we can only move to center
                        {
                            if (canMoveArray[0])
                            {
                                AddNewState(state, i, 0);
                            }
                        }
                    }
                    canMoveArray[state[i]] = false;
                }
                // The second biggest:
                if (state[numDiscs - 2] == 0 && state[numDiscs - 1] == 0)
                {
                    if (canMoveArray[0] && canMoveArray[2])
                    {
                        AddNewState(state, numDiscs - 2, 2);
                    }
                    if (canMoveArray[0] && canMoveArray[3])
                    {
                        AddNewState(state, numDiscs - 2, 3);
                    }
                    canMoveArray[0] = false;
                }
                else if (state[numDiscs - 2] == 0 && state[numDiscs - 1] == 1)
                {
                    if (canMoveArray[0] && canMoveArray[1])
                    {
                        AddNewState(state, numDiscs - 2, 1);
                    }
                    canMoveArray[0] = false;
                }
                else if (state[numDiscs - 2] > 1 && state[numDiscs - 1] == 1)
                {
                    if (canMoveArray[state[numDiscs - 2]] && canMoveArray[0])
                    {
                        AddNewState(state, numDiscs - 2, 0);
                    }
                    canMoveArray[state[numDiscs - 2]] = false;
                }
                // Biggest disk is moved only once
                if (state[numDiscs - 1] == 0)
                {
                    if (canMoveArray[0] && canMoveArray[1])
                    {
                        AddNewState(state, numDiscs - 1, 1);
                        //Console.WriteLine("The biggest is moved!\n");
                    }
                }
            }

            public int ShortestPathForSmallDimension()
        {
            // For each disc we have its peg
            stateArray = new byte[this.numDiscs];
            canMoveArray = new bool[this.numPegs];

            setPrev = new();
            setCurrent = new();
            setNew = new();

            // Set initial and final states for each case
            {
                if (this.type == HanoiType.K13_01)
                {
                    stateArray = ArrayAllEqual(0);
                    finalState = FinalState();
                }
                else if (this.type == HanoiType.K13_12)
                {
                    stateArray = ArrayAllEqual(2);
                    finalState = FinalState();
                }
                else
                {
                    throw new Exception("Hanoi type state is not defined here!");
                }
            }

            currentDistance = 0;
            uint initialState = StateToLong(stateArray); // postavimo vse diske na pozicijo 
            setCurrent.Add(initialState);

            int maxCardinality = 0;
            long maxMemory = 0;
            InitIgnoredStates(type);
            uint solved = 0;


            while (true) // Analiza posameznega koraka (i-tega premika)
            {
                if (maxCardinality < setCurrent.Count)
                    maxCardinality = setCurrent.Count;


               // setCurrent.AsParallel().ForAll(num =>
                 foreach (int num in setCurrent)
                {
                    if (num == finalState)
                    {
                        solved++;
                    }
                    else if (solved == 0)
                    {
                       // lock (this)
                      //  {
                            byte[] tmpState = LongToState(num);

                            switch (type)
                            {
                                case HanoiType.K13_01:
                                    MakeMoveForSmallDimension_K13_01_Fast(tmpState);
                                    break;

                                case HanoiType.K13_12:
                                    MakeMoveForSmallDimension_K13(tmpState);
                                    break;
                            }
                        }
                    }
              //  });


                if (solved != 1)
                {
                    long mem = GC.GetTotalMemory(false);
                    if (maxMemory < mem)
                    {
                        maxMemory = mem;
                    }

                    // Ko se premaknemo iz vseh trenutnih stanj,
                    // pregledamo nova trenutna stanja

                    setPrev = setCurrent;
                    setCurrent = new(setNew);
                    setNew = new();


                    currentDistance++;

                    Console.WriteLine("Current distance: " + currentDistance + "     Maximum cardinality: " + maxCardinality);
                    Console.WriteLine("Memory allocation: " + mem / 1000000 + "MB  \t\t Maximum memory: " + maxMemory / 1000000 + "MB");
                    Console.CursorTop -= 2;
                }
                else
                {
                    return currentDistance;
                }
            }
        }

    }
        public class K13e : Hanoi
        {
            public int ShortestPathForSmallDimension()
            {

                // For each disc we have its peg
                stateArray = new byte[this.numDiscs];
                canMoveArray = new bool[this.numPegs];

                setPrev = new();
                setCurrent = new();
                setNew = new();

                // Set initial and final states for each case
                {
                    if (this.type == HanoiType.K13e_01)
                    {
                        stateArray = ArrayAllEqual(0);
                        finalState = StateAllEqual(1);
                    }
                    else if (this.type == HanoiType.K13e_12)
                    {
                        stateArray = ArrayAllEqual(1);
                        finalState = StateAllEqual(2);
                    }
                    else if (this.type == HanoiType.K13e_23)
                    {
                        stateArray = ArrayAllEqual(2);
                        finalState = StateAllEqual(3);
                    }
                    else if (this.type == HanoiType.K13e_30)
                    {
                        stateArray = ArrayAllEqual(3);
                        finalState = StateAllEqual(0);
                    }
                    else
                    {
                        throw new Exception("Hanoi type state is not defined here!");
                    }
                }

                currentDistance = 0;
                uint initialState = StateToLong(stateArray); // postavimo vse diske na pozicijo 
                setCurrent.Add(initialState);

                int maxCardinality = 0;
                long maxMemory = 0;
                InitIgnoredStates(type);
                uint solved = 0;


                while (true) // Analiza posameznega koraka (i-tega premika)
                {
                    if (maxCardinality < setCurrent.Count)
                        maxCardinality = setCurrent.Count;

                // setCurrent.AsParallel().ForAll(num =>
                //Parallel.ForEach(setCurrent, num => 
                foreach (int num in setCurrent)
                {
                    if (num == finalState)
                    {
                        solved++;
                    }
                    else if (solved == 0)
                    {
                        byte[] tmpState = LongToState(num);
                        MakeMoveForSmallDimension_K13e(tmpState);
                    }
                }
                 //   });


                    if (solved != 1)
                    {
                        long mem = GC.GetTotalMemory(false);
                        if (maxMemory < mem)
                        {
                            maxMemory = mem;
                        }

                        // Ko se premaknemo iz vseh trenutnih stanj,
                        // pregledamo nova trenutna stanja

                        setPrev = setCurrent;
                        setCurrent = new(setNew);
                        setNew = new();


                        currentDistance++;

                        Console.WriteLine("Current distance: " + currentDistance + "     Maximum cardinality: " + maxCardinality);
                        Console.WriteLine("Memory allocation: " + mem / 1000000 + "MB  \t\t Maximum memory: " + maxMemory / 1000000 + "MB");
                        Console.CursorTop -= 2;
                    }
                    else
                    {
                        return currentDistance;
                    }
                }
            }
            private void MakeMoveForSmallDimension_K13e(byte[] state)
            {
                bool[] innerCanMoveArray = new bool[this.numPegs];
                ResetArray(innerCanMoveArray);
                byte[] innerNewState;

                for (int i = 0; i < numDiscs; i++)
                {
                    if (innerCanMoveArray[state[i]])
                    {
                        if (state[i] == 0)
                        {
                            for (byte j = 1; j < numPegs; j++)
                            {
                                if (innerCanMoveArray[j])
                                {
                                    innerNewState = new byte[state.Length];
                                    for (int x = 0; x < state.Length; x++)
                                        innerNewState[x] = state[x];
                                    innerNewState[i] = j;
                                    long innerCurrentState = StateToLong(innerNewState);
                                    // Zaradi takih preverjanj potrebujemo hitro iskanje!
                                    if (!setPrev.Contains(innerCurrentState))
                                    {
                                        {
                                            setNew.Add(innerCurrentState);
                                        }
                                    }
                                }
                            }
                        }
                        else if (state[i] == 1)
                        {
                            if (innerCanMoveArray[0])
                            {
                                innerNewState = new byte[state.Length];
                                for (int x = 0; x < state.Length; x++)
                                    innerNewState[x] = state[x];
                                innerNewState[i] = 0;
                                long innerCurrentState = StateToLong(innerNewState);
                                if (!setPrev.Contains(innerCurrentState))
                                {
                                    {
                                        setNew.Add(innerCurrentState);
                                    }
                                }
                            }
                        }
                        else if (state[i] == 2)
                        {
                            foreach (byte j in new byte[] { 0, 3 })
                            {
                                if (innerCanMoveArray[j])
                                {
                                    innerNewState = new byte[state.Length];
                                    for (int x = 0; x < state.Length; x++)
                                        innerNewState[x] = state[x];
                                    innerNewState[i] = j;
                                    long innerCurrentState = StateToLong(innerNewState);
                                    if (!setPrev.Contains(innerCurrentState))
                                    {
                                        {
                                            setNew.Add(innerCurrentState);
                                        }
                                    }
                                }
                            }
                        }
                        else if (state[i] == 3)
                        {
                            foreach (byte j in new byte[] { 0, 2 })
                            {
                                if (innerCanMoveArray[j])
                                {
                                    innerNewState = new byte[state.Length];
                                    for (int x = 0; x < state.Length; x++)
                                        innerNewState[x] = state[x];
                                    innerNewState[i] = j;
                                    long innerCurrentState = StateToLong(innerNewState);
                                    if (!setPrev.Contains(innerCurrentState))
                                    {
                                        {
                                            setNew.Add(innerCurrentState);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    innerCanMoveArray[state[i]] = false;
                }
            }

        }
        public class K4e : Hanoi
        {
            public int ShortestPathForSmallDimension()
            {

                // For each disc we have its peg
                stateArray = new byte[this.numDiscs];
                canMoveArray = new bool[this.numPegs];

                setPrev = new();
                setCurrent = new();
                setNew = new();

                // Set initial and final states for each case
                {
                    if (this.type == HanoiType.K4e_01)
                    {
                        stateArray = ArrayAllEqual(0);
                        finalState = StateAllEqual(1);
                    }
                    else if (this.type == HanoiType.K4e_12)
                    {
                        stateArray = ArrayAllEqual(1);
                        finalState = StateAllEqual(2);
                    }
                    else if (this.type == HanoiType.K4e_23)
                    {
                        stateArray = ArrayAllEqual(2);
                        finalState = StateAllEqual(3);
                    }
                    else
                    {
                        throw new Exception("Hanoi type state is not defined here!");
                    }
                }

                currentDistance = 0;
                uint initialState = StateToLong(stateArray); // postavimo vse diske na pozicijo 
                setCurrent.Add(initialState);

                int maxCardinality = 0;
                long maxMemory = 0;
                InitIgnoredStates(type);
                uint solved = 0;


                while (true) // Analiza posameznega koraka (i-tega premika)
                {
                    if (maxCardinality < setCurrent.Count)
                        maxCardinality = setCurrent.Count;


                    setCurrent.AsParallel().ForAll(num =>
                    // foreach (int num in setCurrent)
                    {
                        if (num == finalState)
                        {
                            solved++;
                        }
                        else if (solved == 0)
                        {
                            {
                                byte[] tmpState = LongToState(num);
                                MakeMoveForSmallDimension_K4e(tmpState);
                            }
                        }
                        // }
                    });


                    if (solved != 1)
                    {
                        long mem = GC.GetTotalMemory(false);
                        if (maxMemory < mem)
                        {
                            maxMemory = mem;
                        }

                        // Ko se premaknemo iz vseh trenutnih stanj,
                        // pregledamo nova trenutna stanja

                        setPrev = setCurrent;
                        setCurrent = new(setNew);
                        setNew = new();


                        currentDistance++;

                        Console.WriteLine("Current distance: " + currentDistance + "     Maximum cardinality: " + maxCardinality);
                        Console.WriteLine("Memory allocation: " + mem / 1000000 + "MB  \t\t Maximum memory: " + maxMemory / 1000000 + "MB");
                        Console.CursorTop -= 2;
                    }
                    else
                    {
                        return currentDistance;
                    }
                }
            }
            private void MakeMoveForSmallDimension_K4e(byte[] state)
            {
                ResetArray(canMoveArray);

                for (int i = 0; i < numDiscs; i++)
                {
                    if (canMoveArray[state[i]])
                    {
                        if (state[i] == 0)
                        {
                            foreach (byte j in new byte[] { 1, 2, 3 })
                            {
                                if (canMoveArray[j])
                                {
                                    newState = new byte[state.Length];
                                    for (int x = 0; x < state.Length; x++)
                                        newState[x] = state[x];
                                    newState[i] = j;
                                    currentState = StateToLong(newState);
                                    if (!setPrev.Contains(currentState))
                                    {
                                        setNew.Add(currentState);
                                    }
                                }
                            }
                        }
                        else if (state[i] == 1)
                        {
                            foreach (byte j in new byte[] { 0, 2, 3 })
                            {
                                if (canMoveArray[j])
                                {
                                    newState = new byte[state.Length];
                                    for (int x = 0; x < state.Length; x++)
                                        newState[x] = state[x];
                                    newState[i] = j;
                                    currentState = StateToLong(newState);
                                    if (!setPrev.Contains(currentState))
                                    {
                                        setNew.Add(currentState);
                                    }
                                }
                            }
                        }
                        else if (state[i] == 2)
                        {
                            foreach (byte j in new byte[] { 0, 1 })
                            {
                                if (canMoveArray[j])
                                {
                                    newState = new byte[state.Length];
                                    for (int x = 0; x < state.Length; x++)
                                        newState[x] = state[x];
                                    newState[i] = j;
                                    currentState = StateToLong(newState);
                                    if (!setPrev.Contains(currentState))
                                    {
                                        setNew.Add(currentState);
                                    }
                                }
                            }
                        }
                        else if (state[i] == 3)
                        {
                            foreach (byte j in new byte[] { 0, 1 })
                            {
                                if (canMoveArray[j])
                                {
                                    newState = new byte[state.Length];
                                    for (int x = 0; x < state.Length; x++)
                                        newState[x] = state[x];
                                    newState[i] = j;
                                    currentState = StateToLong(newState);
                                    if (!setPrev.Contains(currentState))
                                    {
                                        setNew.Add(currentState);
                                    }
                                }
                            }
                        }
                    }
                    canMoveArray[state[i]] = false;
                }
            }
        }
        public class C4 : Hanoi
        {
            public int ShortestPathForSmallDimension()
            {

                // For each disc we have its peg
                stateArray = new byte[this.numDiscs];
                canMoveArray = new bool[this.numPegs];

                setPrev = new();
                setCurrent = new();
                setNew = new();

                // Set initial and final states for each case
                {
                    if (this.type == HanoiType.C4_01)
                    {
                        stateArray = ArrayAllEqual(0);
                        finalState = StateAllEqual(1);
                    }
                    else if (this.type == HanoiType.C4_12)
                    {
                        stateArray = ArrayAllEqual(1);
                        finalState = StateAllEqual(2);
                    }

                    else
                    {
                        throw new Exception("Hanoi type state is not defined here!");
                    }
                }

                currentDistance = 0;
                uint initialState = StateToLong(stateArray); // postavimo vse diske na pozicijo 
                setCurrent.Add(initialState);

                int maxCardinality = 0;
                long maxMemory = 0;
                InitIgnoredStates(type);
                uint solved = 0;


                while (true) // Analiza posameznega koraka (i-tega premika)
                {
                    if (maxCardinality < setCurrent.Count)
                        maxCardinality = setCurrent.Count;


                    setCurrent.AsParallel().ForAll(num =>
                    // foreach (int num in setCurrent)
                    {
                        if (num == finalState)
                        {
                            solved++;
                        }
                        else if (solved == 0)
                        {
                            {
                                byte[] tmpState = LongToState(num);
                                MakeMoveForSmallDimension_C4(tmpState);
                            }
                        }
                        // }
                    });


                    if (solved != 1)
                    {
                        long mem = GC.GetTotalMemory(false);
                        if (maxMemory < mem)
                        {
                            maxMemory = mem;
                        }

                        // Ko se premaknemo iz vseh trenutnih stanj,
                        // pregledamo nova trenutna stanja

                        setPrev = setCurrent;
                        setCurrent = new(setNew);
                        setNew = new();


                        currentDistance++;

                        Console.WriteLine("Current distance: " + currentDistance + "     Maximum cardinality: " + maxCardinality);
                        Console.WriteLine("Memory allocation: " + mem / 1000000 + "MB  \t\t Maximum memory: " + maxMemory / 1000000 + "MB");
                        Console.CursorTop -= 2;
                    }
                    else
                    {
                        return currentDistance;
                    }
                }
            }
            private void MakeMoveForSmallDimension_C4(byte[] state)
            {
                ResetArray(canMoveArray);

                for (int i = 0; i < numDiscs; i++)
                {
                    if (canMoveArray[state[i]])
                    {
                        if (state[i] == 0)
                        {
                            foreach (byte j in new byte[] { 2, 3 })
                            {
                                if (canMoveArray[j])
                                {
                                    newState = new byte[state.Length];
                                    for (int x = 0; x < state.Length; x++)
                                        newState[x] = state[x];
                                    newState[i] = j;
                                    currentState = StateToLong(newState);
                                    if (!setPrev.Contains(currentState))
                                    {
                                        setNew.Add(currentState);
                                    }
                                }
                            }
                        }
                        else if (state[i] == 1)
                        {
                            foreach (byte j in new byte[] { 2, 3 })
                            {
                                if (canMoveArray[j])
                                {
                                    newState = new byte[state.Length];
                                    for (int x = 0; x < state.Length; x++)
                                        newState[x] = state[x];
                                    newState[i] = j;
                                    currentState = StateToLong(newState);
                                    if (!setPrev.Contains(currentState))
                                    {
                                        setNew.Add(currentState);
                                    }
                                }
                            }
                        }
                        else if (state[i] == 2)
                        {
                            foreach (byte j in new byte[] { 0, 1 })
                            {
                                if (canMoveArray[j])
                                {
                                    newState = new byte[state.Length];
                                    for (int x = 0; x < state.Length; x++)
                                        newState[x] = state[x];
                                    newState[i] = j;
                                    currentState = StateToLong(newState);
                                    if (!setPrev.Contains(currentState))
                                    {
                                        setNew.Add(currentState);
                                    }
                                }
                            }
                        }
                        else if (state[i] == 3)
                        {
                            foreach (byte j in new byte[] { 0, 1 })
                            {
                                if (canMoveArray[j])
                                {
                                    newState = new byte[state.Length];
                                    for (int x = 0; x < state.Length; x++)
                                        newState[x] = state[x];
                                    newState[i] = j;
                                    currentState = StateToLong(newState);
                                    if (!setPrev.Contains(currentState))
                                    {
                                        setNew.Add(currentState);
                                    }
                                }
                            }
                        }
                    }
                    canMoveArray[state[i]] = false;
                }
            }
        }
        public class P4 : Hanoi
        {
            public int ShortestPathForSmallDimension()
            {

                // For each disc we have its peg
                stateArray = new byte[this.numDiscs];
                canMoveArray = new bool[this.numPegs];

                setPrev = new();
                setCurrent = new();
                setNew = new();

                // Set initial and final states for each case
                {
                    if (this.type == HanoiType.P4_01)
                    {
                        stateArray = ArrayAllEqual(0);
                        finalState = StateAllEqual(1);
                    }
                    else if (this.type == HanoiType.P4_12)
                    {
                        stateArray = ArrayAllEqual(1);
                        finalState = StateAllEqual(2);
                    }
                    else if (this.type == HanoiType.P4_23)
                    {
                        stateArray = ArrayAllEqual(2);
                        finalState = StateAllEqual(3);
                    }
                    else if (this.type == HanoiType.P4_31)
                    {
                        stateArray = ArrayAllEqual(3);
                        finalState = StateAllEqual(1);
                    }
                    else
                    {
                        throw new Exception("Hanoi type state is not defined here!");
                    }
                }

                currentDistance = 0;
                uint initialState = StateToLong(stateArray); // postavimo vse diske na pozicijo 
                setCurrent.Add(initialState);

                int maxCardinality = 0;
                long maxMemory = 0;
                InitIgnoredStates(type);
                uint solved = 0;


                while (true) // Analiza posameznega koraka (i-tega premika)
                {
                    if (maxCardinality < setCurrent.Count)
                        maxCardinality = setCurrent.Count;



                    foreach (int num in setCurrent)
                    {
                        if (num == finalState)
                        {
                            solved++;
                        }
                        else if (solved == 0)
                        {
                            byte[] tmpState = LongToState(num);
                            MakeMoveForSmallDimension_P4(tmpState);
                        }
                    }

                    if (solved != 1)
                    {
                        long mem = GC.GetTotalMemory(false);
                        if (maxMemory < mem)
                        {
                            maxMemory = mem;
                        }

                        // Ko se premaknemo iz vseh trenutnih stanj,
                        // pregledamo nova trenutna stanja

                        setPrev = setCurrent;
                        setCurrent = new(setNew);
                        setNew = new();


                        currentDistance++;

                        Console.WriteLine("Current distance: " + currentDistance + "     Maximum cardinality: " + maxCardinality);
                        Console.WriteLine("Memory allocation: " + mem / 1000000 + "MB  \t\t Maximum memory: " + maxMemory / 1000000 + "MB");
                        Console.CursorTop -= 2;
                    }
                    else
                    {
                        return currentDistance;
                    }
                }
            }
            private void MakeMoveForSmallDimension_P4(byte[] state)
            {
                ResetArray(canMoveArray);

                for (int i = 0; i < numDiscs; i++)
                {
                    if (canMoveArray[state[i]])
                    {
                        if (state[i] == 0)
                        {
                            foreach (byte j in new byte[] { 3 })
                            {
                                if (canMoveArray[j])
                                {
                                    newState = new byte[state.Length];
                                    for (int x = 0; x < state.Length; x++)
                                        newState[x] = state[x];
                                    newState[i] = j;
                                    currentState = StateToLong(newState);
                                    if (!setPrev.Contains(currentState))
                                    {
                                        setNew.Add(currentState);
                                    }
                                }
                            }
                        }
                        else if (state[i] == 1)
                        {
                            foreach (byte j in new byte[] { 2 })
                            {
                                if (canMoveArray[j])
                                {
                                    newState = new byte[state.Length];
                                    for (int x = 0; x < state.Length; x++)
                                        newState[x] = state[x];
                                    newState[i] = j;
                                    currentState = StateToLong(newState);
                                    if (!setPrev.Contains(currentState))
                                    {
                                        setNew.Add(currentState);
                                    }
                                }
                            }
                        }
                        else if (state[i] == 2)
                        {
                            foreach (byte j in new byte[] { 1, 3 })
                            {
                                if (canMoveArray[j])
                                {
                                    newState = new byte[state.Length];
                                    for (int x = 0; x < state.Length; x++)
                                        newState[x] = state[x];
                                    newState[i] = j;
                                    currentState = StateToLong(newState);
                                    if (!setPrev.Contains(currentState))
                                    {
                                        setNew.Add(currentState);
                                    }
                                }
                            }
                        }
                        else if (state[i] == 3)
                        {
                            foreach (byte j in new byte[] { 0, 2 })
                            {
                                if (canMoveArray[j])
                                {
                                    newState = new byte[state.Length];
                                    for (int x = 0; x < state.Length; x++)
                                        newState[x] = state[x];
                                    newState[i] = j;
                                    currentState = StateToLong(newState);
                                    if (!setPrev.Contains(currentState))
                                    {
                                        setNew.Add(currentState);
                                    }
                                }
                            }
                        }
                    }
                    canMoveArray[state[i]] = false;
                }
            }

        }

}
