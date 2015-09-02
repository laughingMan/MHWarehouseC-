using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace MHWarehouseCSharp.Tests
{
    [TestFixture]
    public class WarehouseTests
    {
        [Test]
        public void testWhenOneBoxIsAddedToOneRoomThenTheRoomContainsTheBox()
        {
            Room loadingDock = new Room(100);
            Room[] rooms = new Room[] {loadingDock};
            Warehouse testObject = new Warehouse(rooms);
            Box box1 = new Box("box1", 10);

            Box[] boxes = new Box[] {box1};
            testObject.addBoxes(boxes);

            Assert.AreEqual(loadingDock.boxes, boxes);
        }

        [Test]
        public void testWhenBoxesAreAddedThenTheyAreAddedToFirstRoomFirstUntilCapacityIsReached()
        {
            Room loadingDock = new Room(100);
            Room mainStorage = new Room(1000);
            Box box1 = new Box("box1", 10);
            Box box2 = new Box("box2", 70);
            Box box3 = new Box("box3", 15);
            Box box4 = new Box("box4", 10);

            Warehouse testObject = new Warehouse(new Room[] {loadingDock, mainStorage});

            testObject.addBoxes(new Box[] {box1, box2, box3, box4});

            Assert.AreEqual(new Box[] {box1, box2, box3}, loadingDock.boxes);
            Assert.AreEqual(new Box[] {box4}, mainStorage.boxes);
        }

        [Test]
        public void testWhenBoxOver50VolumeIsLoadedThenItIsNotLoadedInARoomRequiringStairs()
        {
            Room basement = new Room(1000, true);
            Room mainStorage = new Room(1000, false);
            Box box1 = new Box("box1", 10);
            Box box2 = new Box("box2", 50);
            Box box3 = new Box("box3", 51);
            Box box4 = new Box("box4", 10);

            Warehouse testObject = new Warehouse(new Room[] {basement, mainStorage});

            testObject.addBoxes(new Box[] {box1, box2, box3, box4});

            Assert.AreEqual(new Box[] {box1, box2, box4}, basement.boxes);
            Assert.AreEqual(new Box[] {box3}, mainStorage.boxes);
        }

        [Test]
        public void testWhenBoxesExceedCapacityThenFinalBoxesAreRejected()
        {
            Room loadingDock = new Room(100);
            Box box1 = new Box("box1", 40);
            Box box2 = new Box("box2", 40);
            Box box3 = new Box("box3", 40);
            Box box4 = new Box("box4", 10);

            Warehouse testObject = new Warehouse(new Room[] {loadingDock});

            Box[] rejectedBoxes = testObject.addBoxes(new Box[] {box1, box2, box3, box4});

            Assert.AreEqual(new Box[] {box1, box2, box4}, loadingDock.boxes);
            Assert.AreEqual(new Box[] {box3}, rejectedBoxes);
        }

        [Test]
        public void testWhenChemicalBoxIsLoadedItIsLoadedInSafeRoom()
        {
            Room loadingDock = new Room(100, false, HazmatFlags.NONE);
            Room chemStorage = new Room(100, false, HazmatFlags.CHEMICAL);
            Box box1 = new Box("box1", 10, HazmatFlags.NONE);
            Box box2 = new Box("box2", 10, HazmatFlags.CHEMICAL);
            Box box3 = new Box("box3", 10, HazmatFlags.NONE);

            Warehouse testObject = new Warehouse(new Room[] {loadingDock, chemStorage});

            Box[] rejectedBoxes = testObject.addBoxes(new Box[] {box1, box2, box3});

            Assert.AreEqual(new Box[] {box1, box3}, loadingDock.boxes);
            Assert.AreEqual(new Box[] {box2}, chemStorage.boxes);
            Assert.AreEqual(new Box[0], rejectedBoxes);
        }

        [Test]
        public void testWhenHazmatHasNoSafeRoomThenItIsRejected()
        {
            Room loadingDock = new Room(100, false, HazmatFlags.NONE);
            Room mainStorage = new Room(1000, false, HazmatFlags.NONE);
            Box box1 = new Box("box1", 10, HazmatFlags.NONE);
            Box box2 = new Box("box2", 10, HazmatFlags.CHEMICAL);
            Box box3 = new Box("box3", 10, HazmatFlags.NONE);

            Warehouse testObject = new Warehouse(new Room[] {loadingDock, mainStorage});

            Box[] rejectedBoxes = testObject.addBoxes(new Box[] {box1, box2, box3});

            Assert.AreEqual(new Box[] {box1, box3}, loadingDock.boxes);
            Assert.AreEqual(new Box[0], mainStorage.boxes);
            Assert.AreEqual(rejectedBoxes, new Box[] {box2});
        }

        [Test]
        public void testChemicalBoxCannotGoInANuclearRoom()
        {
            Room nuclearStorage = new Room(100, false, HazmatFlags.NUCLEAR);
            Box box1 = new Box("box1", 10, HazmatFlags.NONE);
            Box box2 = new Box("box2", 10, HazmatFlags.CHEMICAL);
            Box box3 = new Box("box3", 10, HazmatFlags.NONE);

            Warehouse testObject = new Warehouse(new Room[] {nuclearStorage});

            Box[] rejectedBoxes = testObject.addBoxes(new Box[] {box1, box2, box3});

            Assert.AreEqual(new Box[] {box1, box3}, nuclearStorage.boxes);
            Assert.AreEqual(rejectedBoxes, new Box[] { box2 });
        }

        [Test]
        public void testDifferentHazmatBoxesCanBeStoredInDifferentRoomsWhileStillRespectingSizeAndStairs()
        {
            Room loadingDock = new Room(100, false, HazmatFlags.NONE);
            Room chemLoft = new Room(100, true, HazmatFlags.CHEMICAL);
            Room vault = new Room(150, false, HazmatFlags.CHEMICAL | HazmatFlags.NUCLEAR);
            Box box1 = new Box("box1", 10, HazmatFlags.CHEMICAL);
            Box box2 = new Box("box2", 60, HazmatFlags.CHEMICAL);
            Box box3 = new Box("box3", 10, HazmatFlags.NUCLEAR);
            Box box4 = new Box("box4", 10, HazmatFlags.NUCLEAR | HazmatFlags.CHEMICAL);
            Box box5 = new Box("box5", 50, HazmatFlags.CHEMICAL);
            Box box6 = new Box("box6", 50, HazmatFlags.CHEMICAL);

            Warehouse testObject = new Warehouse(new Room[] {loadingDock, chemLoft, vault});

            Box[] rejectedBoxes = testObject.addBoxes(new Box[] {box1, box2, box3, box4, box5, box6});

            Assert.AreEqual(loadingDock.boxes, new Box[0]);
            Assert.AreEqual(new Box[] {box1, box5}, chemLoft.boxes);
            Assert.AreEqual(new Box[] {box2, box3, box4, box6}, vault.boxes);
            Assert.True(rejectedBoxes.Length == 0);
        }

        [Test]
        public void testBoxesAreNotPlacedSuchThatAHazmatWillHaveNoPlaceToGoWhenThereIsEnoughRoom()
        {
            Room vault = new Room(150, false, HazmatFlags.CHEMICAL | HazmatFlags.NUCLEAR);
            Room mainStorage = new Room(1000, false, HazmatFlags.NONE);
            Box box1 = new Box("box1", 60, HazmatFlags.NONE);
            Box box2 = new Box("box2", 60, HazmatFlags.NONE);
            Box box3 = new Box("box3", 60, HazmatFlags.NONE);
            Box box4 = new Box("box4", 60, HazmatFlags.NONE);
            Box box5 = new Box("box5", 60, HazmatFlags.CHEMICAL);

            Warehouse testObject = new Warehouse(new Room[] {vault, mainStorage});

            Box[] rejectedBoxes = testObject.addBoxes(new Box[] {box1, box2, box3, box4, box5});

            Assert.AreEqual(new Box[] {box1, box5}, vault.boxes);
            Assert.AreEqual(new Box[] {box2, box3, box4}, mainStorage.boxes);
            Assert.True(rejectedBoxes.Length == 0);
        }

        [Test]
        public void testOrderForBoxesIsPreservedWhenThereIsEnoughRoom()
        {
            Room vault = new Room(150, false, HazmatFlags.CHEMICAL | HazmatFlags.NUCLEAR);
            Room mainStorage = new Room(1000, false, HazmatFlags.NONE);
            Box box1 = new Box("box1", 60, HazmatFlags.NONE);
            Box box2 = new Box("box2", 60, HazmatFlags.NONE);
            Box box3 = new Box("box3", 60, HazmatFlags.NONE);
            Box box4 = new Box("box4", 30, HazmatFlags.NONE);
            Box box5 = new Box("box5", 60, HazmatFlags.CHEMICAL);

            Warehouse testObject = new Warehouse(new Room[] {vault, mainStorage});

            Box[] rejectedBoxes = testObject.addBoxes(new Box[] {box1, box2, box3, box4, box5});

            Assert.AreEqual(new Box[] {box1, box4, box5}, vault.boxes);
            Assert.AreEqual(new Box[] {box2, box3}, mainStorage.boxes);
            Assert.True(rejectedBoxes.Length == 0);
        }
    }
}

 