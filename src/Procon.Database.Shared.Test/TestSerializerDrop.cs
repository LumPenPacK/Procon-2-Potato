﻿using Procon.Database.Shared.Builders.Methods.Schema;
using Procon.Database.Shared.Builders.Statements;

namespace Procon.Database.Shared.Test {
    public abstract class TestSerializerDrop {
        #region TestDropDatabaseProcon

        protected IDatabaseObject TestDropDatabaseProconExplicit = new Drop()
            .Database(new Shared.Builders.Database() {
                Name = "Procon"
            });

        protected IDatabaseObject TestDropDatabaseProconImplicit = new Drop()
            .Database("Procon");

        public abstract void TestDropDatabaseProcon();

        #endregion

        #region TestDropTablePlayer

        protected IDatabaseObject TestDropTablePlayerExplicit = new Drop()
            .Collection(new Collection() {
                Name = "Player"
            });

        protected IDatabaseObject TestDropTablePlayerImplicit = new Drop()
            .Collection("Player");

        public abstract void TestDropTablePlayer();

        #endregion
    }
}