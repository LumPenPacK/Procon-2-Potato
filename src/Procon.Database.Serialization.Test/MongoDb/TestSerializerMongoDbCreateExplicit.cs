﻿using System.Linq;
using NUnit.Framework;

namespace Procon.Database.Serialization.Test.MongoDb {
    [TestFixture]
    public class TestSerializerMongoDbCreateExplicit : TestSerializerCreate {
        [Test]
        public override void TestCreateDatabaseProcon() {
            ISerializer serializer = new SerializerMongoDb();
            ICompiledQuery serialized = serializer.Parse(this.TestCreateDatabaseProconExplicit).Compile();

            Assert.AreEqual(@"create", serialized.Methods.First());
            Assert.AreEqual(@"Procon", serialized.Databases.First());
        }

        [Test, Ignore]
        public override void TestCreatePlayerWithFieldStringName() {
            
        }

        [Test, Ignore]
        public override void TestCreatePlayerWithFieldStringSpecifiedLengthName() {
            
        }

        [Test, Ignore]
        public override void TestCreatePlayerWithFieldStringSpecifiedLengthNameAndFieldIntegerScore() {
            
        }

        [Test, Ignore]
        public override void TestCreatePlayerWithFieldStringSpecifiedLengthNameNotNullAndFieldIntegerScore() {
            
        }

        [Test, Ignore]
        public override void TestCreatePlayerWithFieldIntegerScoreUnsigned() {
            
        }

        [Test, Ignore]
        public override void TestCreatePlayerWithFieldIntegerScoreUnsignedAutoIncrement() {
            
        }

        [Test]
        public override void TestCreatePlayerWithFieldStringNameWithIndexOnName() {
            ISerializer serializer = new SerializerMongoDb();
            ICompiledQuery serialized = serializer.Parse(this.TestCreatePlayerWithFieldStringNameWithIndexOnNameExplicit).Compile();

            Assert.AreEqual(@"create", serialized.Methods.First());
            Assert.AreEqual(@"{""Name"":1}", serialized.Indices.First());
        }

        [Test]
        public override void TestCreatePlayerWithFieldStringNameWithIndexOnNameDescending() {
            ISerializer serializer = new SerializerMongoDb();
            ICompiledQuery serialized = serializer.Parse(this.TestCreatePlayerWithFieldStringNameWithIndexOnNameDescendingExplicit).Compile();

            Assert.AreEqual(@"create", serialized.Methods.First());
            Assert.AreEqual(@"{""Name"":-1}", serialized.Indices.First());
        }

        [Test]
        public override void TestCreatePlayerWithFieldStringNameWithIndexOnNameScore() {
            ISerializer serializer = new SerializerMongoDb();
            ICompiledQuery serialized = serializer.Parse(this.TestCreatePlayerWithFieldStringNameWithIndexOnNameScoreExplicit).Compile();

            Assert.AreEqual(@"create", serialized.Methods.First());
            Assert.AreEqual(@"{""Name"":1}", serialized.Indices.First());
            Assert.AreEqual(@"{""Score"":1}", serialized.Indices.Last());
        }

        [Test]
        public override void TestCreatePlayerWithFieldStringNameWithIndexOnNameScoreCompound() {
            ISerializer serializer = new SerializerMongoDb();
            ICompiledQuery serialized = serializer.Parse(this.TestCreatePlayerWithFieldStringNameWithIndexOnNameScoreCompoundExplicit).Compile();

            Assert.AreEqual(@"create", serialized.Methods.First());
            Assert.AreEqual(@"{""Name"":1,""Score"":1}", serialized.Indices.First());
        }

        [Test]
        public override void TestCreatePlayerWithFieldStringNameWithIndexOnNameScoreDescendingCompound() {
            ISerializer serializer = new SerializerMongoDb();
            ICompiledQuery serialized = serializer.Parse(this.TestCreatePlayerWithFieldStringNameWithIndexOnNameScoreDescendingCompoundExplicit).Compile();

            Assert.AreEqual(@"create", serialized.Methods.First());
            Assert.AreEqual(@"{""Name"":1,""Score"":-1}", serialized.Indices.First());
        }
    }
}
