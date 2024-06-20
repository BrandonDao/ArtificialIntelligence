namespace CSPSolver
{
    public class Program
    {

        public class CSPSolver<TID, TValue>()
            where TID : Enum
        {
            public class Variable(TID id, HashSet<TValue> domain)
            {
                public TID ID { get; set; } = id;
                public TValue? Value { get; set; }
                public HashSet<TValue> Domain { get; set; } = domain;
                public Dictionary<Variable[], Predicate<Variable[]>> Constraints { get; set; } = [];
            }

            private readonly Dictionary<TID, Variable> variableByID = [];

            public bool TryCreateVariable(TID id, HashSet<TValue> domain, out Variable? createdVariable)
            {
                createdVariable = null;

                if (variableByID.ContainsKey(id)) return false;

                createdVariable = new Variable(id, domain);
                variableByID.Add(id, createdVariable);

                return true;
            }

            public void AddUnaryConstraint(TID id, Predicate<Variable> condition)
                => variableByID[id].Domain.RemoveWhere((TValue val) => !condition.Invoke(variableByID[id]));
            public void AddUnaryConstraint(Variable var, Predicate<Variable> condition)
                => var.Domain.RemoveWhere((TValue val) => !condition.Invoke(var));

            public void AddBinaryConstraint(TID id, TID[] conditionIDs, Predicate<Variable[]> condition)
            {
                var otherVars = new Variable[conditionIDs.Length];
                for(int i = 0; i < conditionIDs.Length; i++)
                {
                    otherVars[i] = variableByID[conditionIDs[i]];
                }
                variableByID[id].Constraints.Add(otherVars, condition);
            }

            public HashSet<TID> Solve()
            {
                throw new NotImplementedException();
            }
        }

        private enum Vars { T, W, O, F, U, R };

        private static void Main()
        {







            //CSPSolver<Vars, int> solver = new();

            //// Unary
            //static bool IsEven(CSPSolver<Vars, int>.Variable var) => (var.Value & 1) == 0;
            //// Binary
            //static bool AreNotEqual(CSPSolver<Vars, int>.Variable[] vars)
            //{
            //    for(int i = 0; i < vars.Length; i++)
            //    {
            //        for(int j = i + 1; j < vars.Length; j++)
            //        {
            //            if (vars[i].Value == vars[j].Value) return false;
            //        }
            //    }
            //    return true;
            //}
            //static bool AreSum((int a, int b) vals) => vals.a == vals.b << 1;

            //solver.TryCreateVariable(Vars.T, Enumerable.Range(0, 10).ToHashSet(), out var T);
            //solver.TryCreateVariable(Vars.W, Enumerable.Range(0, 10).ToHashSet(), out var W);
            //solver.TryCreateVariable(Vars.O, Enumerable.Range(0, 10).ToHashSet(), out var O);
            //solver.TryCreateVariable(Vars.F, Enumerable.Range(0, 10).ToHashSet(), out var F);
            //solver.TryCreateVariable(Vars.U, Enumerable.Range(0, 10).ToHashSet(), out var U);
            //solver.TryCreateVariable(Vars.R, Enumerable.Range(0, 10).ToHashSet(), out var R);

            //solver.AddUnaryConstraint(T, (var) => var.Value >= 5);

            //solver.AddUnaryConstraint(Vars.R, IsEven);

            //solver.AddBinaryConstraint(Vars.R, Vars.O, AreSum);
            //solver.AddBinaryConstraint(Vars.U, Vars.O, AreSum);

            //// All variables are unique
            //solver.AddBinaryConstraint(Vars.T, Vars.W, AreNotEqual);
            //solver.AddBinaryConstraint(Vars.T, Vars.O, AreNotEqual);
            //solver.AddBinaryConstraint(Vars.T, Vars.F, AreNotEqual);
            //solver.AddBinaryConstraint(Vars.T, Vars.U, AreNotEqual);
            //solver.AddBinaryConstraint(Vars.T, Vars.R, AreNotEqual);

            //solver.AddBinaryConstraint(Vars.W, Vars.O, AreNotEqual);
            //solver.AddBinaryConstraint(Vars.W, Vars.F, AreNotEqual);
            //solver.AddBinaryConstraint(Vars.W, Vars.U, AreNotEqual);
            //solver.AddBinaryConstraint(Vars.W, Vars.R, AreNotEqual);

            //solver.AddBinaryConstraint(Vars.O, Vars.F, AreNotEqual);
            //solver.AddBinaryConstraint(Vars.O, Vars.U, AreNotEqual);
            //solver.AddBinaryConstraint(Vars.O, Vars.R, AreNotEqual);

            //solver.AddBinaryConstraint(Vars.F, Vars.U, AreNotEqual);
            //solver.AddBinaryConstraint(Vars.F, Vars.R, AreNotEqual);

            //solver.AddBinaryConstraint(Vars.U, Vars.R, AreNotEqual);
        }
    }
}