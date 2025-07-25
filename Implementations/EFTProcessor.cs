﻿using dnlib.DotNet;
using Spectre.Console;

// For Lone Tarkov

namespace TarkovDumper.Implementations
{
    public sealed class EFTProcessor : Processor
    {
        private const string ASSEMBLY_INPUT_PATH = @"W:\current projects\tarky-dump\input\DLL\tarkov\Assembly-CSharp.dll";
        private const string DUMP_INPUT_PATH = @"W:\current projects\tarky-dump\input\tarkov\dump.txt";
        private const string SDK_OUTPUT_PATH = @"W:\current projects\tarky-dump\output\Ltarkov\SDK.cs";

        public EFTProcessor() : base(ASSEMBLY_INPUT_PATH, DUMP_INPUT_PATH) { }

        public override void Run(StatusContext ctx)
        {
            base.Run(ctx);
            var structGenerator_classNames = new StructureGenerator("ClassNames");
            var structGenerator_offsets = new StructureGenerator("Offsets");
            var structGenerator_enums = new StructureGenerator("Enums");
            ProcessClassNames(ctx, structGenerator_classNames);
            ProcessOffsets(ctx, structGenerator_offsets);
            ProcessEnums(ctx, structGenerator_enums);

            AnsiConsole.Clear();

            var sgList = new List<StructureGenerator>()
            {
                structGenerator_classNames,
                structGenerator_offsets,
                structGenerator_enums,
            };
            AnsiConsole.WriteLine(StructureGenerator.GenerateNamespace("SDK", sgList));
            AnsiConsole.WriteLine(StructureGenerator.GenerateReports(sgList));

            string plainSDK = StructureGenerator.GenerateNamespace("SDK", sgList, false);
            File.WriteAllText(SDK_OUTPUT_PATH, plainSDK);
        }

        private void ProcessClassNames(StatusContext ctx, StructureGenerator structGenerator)
        {
            void SetVariableStatus(string variable)
            {
                LastStepName = variable;
                ctx.Status(variable);
            }

            /*
             
            format for class finder

            entity = method name that anchors the finder to the class

            variable is just the field name we decide that equals the serial number name of the class in dnspy. Often ClassName is used.

            SetVariableStatus(variable); Resets the finder module to target our anchor (entity)

            create a new StructureGenerator object where the parameter is the name you want the struct in sdk.cs

            nestedStruct.AddClassName() should be first, it actually uses the finder module to get the method defined in string entity.

            nestedStruct.AddString() add a string type field to the struct, defining the exact method name to target

            structGenerator.AddStruct(nestedStruct)  ProcessClassNames parameter object to add our nested structure to the main file.
             
             */

            {
                string entity = "IsLocalStreamer";
                string variable = "ClassName";
                SetVariableStatus(variable);

                StructureGenerator nestedStruct = new("StreamerMode");
                nestedStruct.AddString("MethodName", "IsLocalStreamer");

                nestedStruct.AddClassName(_dnlibHelper.FindClassWithEntityName(entity, DnlibHelper.SearchType.Method), variable, entity);

                structGenerator.AddStruct(nestedStruct);
            }

            { // Thank you SoTMaulder for the paste !
                string name = "EFT.UI.DragAndDrop.GridItemView";
                string entity = "itemComponent.Nickname.SubstringIfNecessary";
                string variable = "MethodName";
                SetVariableStatus(variable);

                StructureGenerator nestedStruct = new("GridItemView");

                var fClass = _dnlibHelper.FindClassByTypeName(name);
                var fMethod = _dnlibHelper.FindMethodThatContains(_decompiler_Basic, fClass, entity);

                nestedStruct.AddMethodName(fMethod, variable, entity);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string entity = "SetUpSpawnInfo";
                string variable = "ClassName";
                SetVariableStatus(variable);

                StructureGenerator nestedStruct = new("FixWildSpawnType");
                nestedStruct.AddString("MethodName", entity);

                nestedStruct.AddClassName(_dnlibHelper.FindClassWithEntityName(entity, DnlibHelper.SearchType.Method), variable, entity);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string entity = "EmptyResponseRepeatDelay";
                string variable = "ClassName";
                SetVariableStatus(variable);

                StructureGenerator nestedStruct = new("NetworkContainer");

                nestedStruct.AddClassName(_dnlibHelper.FindClassWithEntityName(entity, DnlibHelper.SearchType.Property), variable, entity);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "AmmoTemplate";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity = "get_LoadUnloadModifier";
                var fClass = _dnlibHelper.FindClassWithEntityName(entity, DnlibHelper.SearchType.Method);
                nestedStruct.AddClassName(fClass, "ClassName", entity);

                var fMethod = _dnlibHelper.FindMethodByName(fClass, entity);
                nestedStruct.AddMethodName(fMethod, "MethodName", entity);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "NoMalfunctions";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity = "GetMalfunctionState";

                var fClass = _dnlibHelper.FindClassWithEntityName(entity, DnlibHelper.SearchType.Method);
                var fMethod = _dnlibHelper.FindMethodByName(fClass, entity);

                nestedStruct.AddClassName(fClass, "ClassName", entity, true);
                nestedStruct.AddMethodName(fMethod, "GetMalfunctionState", "N/A");

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "GymHack";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity = "EFT.Hideout.ShrinkingCircleQTE";

                var fClass = _dnlibHelper.FindClassByTypeName(entity);
                var fMethod = _dnlibHelper.FindMethodByName(fClass, @"\uE001");

                nestedStruct.AddClassName(fClass, "ClassName", entity, true);
                nestedStruct.AddMethodName(fMethod, "MethodName", fMethod.Name);

                structGenerator.AddStruct(nestedStruct);
            }


            {
                string entity = "get_OpticCameraManager";
                string variable = "ClassName";
                SetVariableStatus(variable);

                StructureGenerator nestedStruct = new("OpticCameraManagerContainer");

                nestedStruct.AddClassName(_dnlibHelper.FindClassWithEntityName(entity, DnlibHelper.SearchType.Method), variable, entity);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ScreenManager";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity = "InitChatScreen";

                var fClass = _dnlibHelper.FindClassWithEntityName(entity, DnlibHelper.SearchType.Method);
                nestedStruct.AddClassName(fClass, "ClassName", entity);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string variable = "ClassName";
                SetVariableStatus(variable);

                StructureGenerator nestedStruct = new("FirearmController");

                var fClass = _dnlibHelper.FindClassByTypeName("FirearmController");

                nestedStruct.AddClassName(fClass, variable, "N/A", true);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string entity = "EFT.Animations.ProceduralWeaponAnimation";
                string variable = "ClassName";
                SetVariableStatus(variable);

                StructureGenerator nestedStruct = new("ProceduralWeaponAnimation");

                var fClass = _dnlibHelper.FindClassByTypeName(entity);
                nestedStruct.AddClassName(fClass, variable, entity, true);

                entity = "get_ShotNeedsFovAdjustments";
                variable = "MethodName";
                SetVariableStatus(variable);

                var fMethod = _dnlibHelper.FindMethodByName(fClass, entity);
                nestedStruct.AddMethodName(fMethod, variable, entity);

                structGenerator.AddStruct(nestedStruct);
            }
        }

        private void ProcessOffsets(StatusContext ctx, StructureGenerator structGenerator)
        {
            void SetVariableStatus(string variable)
            {
                LastStepName = variable;
                ctx.Status(variable);
            }

            TypeDef MenuOperationClass = default;

            {
                string name = "TarkovApplication";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);
                const string ClassName = "EFT.TarkovApplication";
                string entity = "MenuOperation";

                {
                    MenuOperationClass = _dnlibHelper.FindClassWithEntityName("StopAfkMonitor", DnlibHelper.SearchType.Method);
                    var offset = _dumpParser.FindOffsetByTypeName(ClassName, $"-.{MenuOperationClass.Humanize()}");
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            string AfkMonitorClassName = default;

            {
                string name = "MenuOperation";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "AfkMonitor";

                    var fMethod = _dnlibHelper.FindMethodByName(MenuOperationClass, "StopAfkMonitor");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);
                    AfkMonitorClassName = fField.GetTypeName();
                    var offset = _dumpParser.FindOffsetByName(MenuOperationClass.Humanize(), fField.Humanize());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "AfkMonitor";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                var fClass = _dnlibHelper.FindClassByTypeName(AfkMonitorClassName);

                {
                    entity = "Delay";

                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "Start");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod, 3);
                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), fField.Humanize());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "GameWorld";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.GameWorld";

                {
                    entity = "LootMaskObstruction";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(className);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_LootMaskObstruction");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);

                }

                {
                    entity = "Location";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(className);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_LocationId");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> TransitControllerOffset = default;
            DumpParser.Result<DumpParser.OffsetData> ClientShellingControllerOffset = default;

            {
                string name = "ClientLocalGameWorld";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "ExfilController";

                    // Find the class that has all of these fields
                    List<DumpParser.EntitySearchListEntry> searchEntities = new()
                    {
                        new("EFT.Interactive.ExfiltrationPoint[]", DumpParser.SearchType.TypeName),
                        new("EFT.Interactive.ScavExfiltrationPoint[]", DumpParser.SearchType.TypeName),
                    };

                    string className = _dumpParser.FindOffsetGroupWithEntities(searchEntities);
                    var offset = _dumpParser.FindOffsetByTypeName(name, className);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "TransitController";

                    var fClass = _dnlibHelper.FindClassWithEntityName("DisableTransitPoints", DnlibHelper.SearchType.Method);
                    TransitControllerOffset = _dumpParser.FindOffsetByTypeName(name, $"-.{fClass.Humanize()}");
                    nestedStruct.AddOffset(entity, TransitControllerOffset);
                }

                {
                    entity = "BtrController";

                    var fClass = _dnlibHelper.FindClassWithEntityName("BotShooterBtr", DnlibHelper.SearchType.Property);
                    var offset = _dumpParser.FindOffsetByTypeName(name, $"-.{fClass.Humanize()}");
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "ClientShellingController";

                    var fClass = _dnlibHelper.FindClassByTypeName("EFT.GameWorld");
                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "get_ClientShellingController");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    ClientShellingControllerOffset = _dumpParser.FindOffsetByName(name, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, ClientShellingControllerOffset);
                }

                {
                    entity = "SynchronizableObjectLogicProcessor";

                    var fClass = _dnlibHelper.FindClassWithEntityName("GetAlivePlayerByProfileID", DnlibHelper.SearchType.Method);
                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "get_SynchronizableObjectLogicProcessor");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(name, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "LocationId";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName("EFT.GameWorld");
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_LocationId");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(name, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "LootList";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "RegisteredPlayers";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "BorderZones";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "MainPlayer";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Grenades";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "TransitController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!TransitControllerOffset.Success)
                {
                    nestedStruct.AddOffset(name, TransitControllerOffset);
                    goto end;
                }

                {
                    entity = "TransitPoints";

                    var offset = _dumpParser.FindOffsetByTypeName(TransitControllerOffset.Value.TypeName, "System.Collections.Generic.Dictionary<Int32, TransitPoint>");
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ClientShellingController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!ClientShellingControllerOffset.Success)
                {
                    nestedStruct.AddOffset(name, ClientShellingControllerOffset);
                    goto end;
                }

                {
                    entity = "ActiveClientProjectiles";

                    var offset = _dumpParser.FindOffsetByName(ClientShellingControllerOffset.Value.TypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ArtilleryProjectileClient";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "CommonAssets.Scripts.ArtilleryShelling.Client.ArtilleryProjectileClient";

                var ThisClass = _dnlibHelper.FindClassByTypeName(className);

                {
                    entity = "IsActive";

                    var fMethod = _dnlibHelper.FindMethodByName(ThisClass, "Update");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Position";

                    const string searchString = " = Vector3.zero;";
                    var fMethod = _dnlibHelper.FindMethodThatContains(_decompiler_Basic, ThisClass, searchString);
                    var decompiledMethod = _decompiler_Basic.DecompileClassMethod(ThisClass, fMethod.Humanize());
                    var fFieldName = TextHelper.FindSubstringAndGoBackwards(decompiledMethod.Body, searchString, '.');

                    var offset = _dumpParser.FindOffsetByName(className, fFieldName);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> TransitPointOffset = default;

            {
                string name = "TransitPoint";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Interactive.TransitPoint";

                {
                    entity = "parameters";

                    TransitPointOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, TransitPointOffset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "TransitParameters";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!TransitPointOffset.Success)
                {
                    nestedStruct.AddOffset(name, TransitPointOffset);
                    goto end;
                }

                {
                    entity = "name";

                    var offset = _dumpParser.FindOffsetByName(TransitPointOffset.Value.TypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "location";

                    var offset = _dumpParser.FindOffsetByName(TransitPointOffset.Value.TypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "description";

                    var offset = _dumpParser.FindOffsetByName(TransitPointOffset.Value.TypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "SynchronizableObject";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.SynchronizableObjects.SynchronizableObject";

                {
                    entity = "Type";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "SynchronizableObjectLogicProcessor";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "SynchronizableObjects";

                    var fClass = _dnlibHelper.FindClassWithEntityName("GetSyncObjectStrategyByType", DnlibHelper.SearchType.Method);

                    var decompiled = _decompiler_Basic.DecompileClassMethod(fClass, "InitStaticObject");
                    string fField = TextHelper.FindSubstringAndGoBackwards(decompiled.Body, ".Add", '.');

                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), fField);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "TripwireSynchronizableObject";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.SynchronizableObjects.TripwireSynchronizableObject";

                {
                    entity = "_tripwireState";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<GrenadeTemplateId>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<FromPosition>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<ToPosition>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "MineDirectional";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "Mines";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "MineData";

                    var offset = _dumpParser.FindOffsetByName(name, "_mineData");
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "MineSettings";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string structName = "MineDirectional.MineSettings";

                {
                    entity = "_maxExplosionDistance";

                    var offset = _dumpParser.FindOffsetByName(structName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_directionalDamageAngle";

                    var offset = _dumpParser.FindOffsetByName(structName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "BorderZone";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Interactive.BorderZone";

                {
                    entity = "Description";

                    TypeDef fClass = _dnlibHelper.FindClassByTypeName(className);
                    MethodDef fMethod = _dnlibHelper.FindMethodByName(fClass, "get_Description");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_extents";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "LevelSettings";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "AmbientMode";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "EquatorColor";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "GroundColor";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "BtrController";
                SetVariableStatus(name);
                var fClass = _dnlibHelper.FindClassWithEntityName("BotShooterBtr", DnlibHelper.SearchType.Property);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "BtrView";

                    var offset = _dumpParser.FindOffsetByTypeName(fClass.Humanize(), "EFT.Vehicle.BTRView");
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            string BtrTurretClassName = default;

            {
                string name = "BTRView";
                SetVariableStatus(name);
                const string className = "EFT.Vehicle.BTRView";

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "turret";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    BtrTurretClassName = offset.Value.TypeName;
                    nestedStruct.AddOffset(entity, offset);
                }
                {
                    entity = "_targetPosition";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "BTRTurretView";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "AttachedBot";

                    var offset = _dumpParser.FindOffsetByTypeName(BtrTurretClassName, "System.ValueTuple<ObservedPlayerView, Boolean>");
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "EFTHardSettings";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "DecelerationSpeed";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "LOOT_RAYCAST_DISTANCE";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "DOOR_RAYCAST_DISTANCE";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "STOP_AIMING_AT";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "MOUSE_LOOK_HORIZONTAL_LIMIT";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ExfilController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                // Find the class that has all of these fields
                List<DumpParser.EntitySearchListEntry> searchEntities = new()
                {
                    new("EFT.Interactive.ExfiltrationPoint[]", DumpParser.SearchType.TypeName),
                    new("EFT.Interactive.ScavExfiltrationPoint[]", DumpParser.SearchType.TypeName),
                    new("EFT.Interactive.SecretExfiltrations.SecretExfiltrationPoint[]", DumpParser.SearchType.TypeName)
                };

                string offsetControllerClassName = _dumpParser.FindOffsetGroupWithEntities(searchEntities);

                {
                    entity = "ExfiltrationPointArray";

                    var offset = _dumpParser.FindOffsetByTypeName(offsetControllerClassName, "EFT.Interactive.ExfiltrationPoint[]");
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "ScavExfiltrationPointArray";

                    var offset = _dumpParser.FindOffsetByTypeName(offsetControllerClassName, "EFT.Interactive.ScavExfiltrationPoint[]");
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "SecretExfiltrationPointArray";

                    var offset = _dumpParser.FindOffsetByTypeName(offsetControllerClassName, "EFT.Interactive.SecretExfiltrations.SecretExfiltrationPoint[]");
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "Exfil";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Interactive.ExfiltrationPoint";

                {
                    entity = "Settings";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "EligibleEntryPoints";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_status";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ScavExfil";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Interactive.ScavExfiltrationPoint";

                {
                    entity = "EligibleIds";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ExfilSettings";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Interactive.ExitTriggerSettings";

                {
                    entity = "Name";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "GenericCollectionContainer";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity = "Grenades";

                var fOffset = _dumpParser.FindOffsetByName("ClientLocalGameWorld", entity);
                if (!fOffset.Success)
                {
                    nestedStruct.AddOffset(entity, fOffset);
                    goto end;
                }

                {
                    entity = "List";

                    string typeName = fOffset.Value.TypeName.Split('<')[0];
                    var offset = _dumpParser.FindOffsetByTypeName(typeName, "System.Collections.Generic.List<Var>");
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "Grenade";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Grenade";

                {
                    entity = "IsDestroyed";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName("Throwable");
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "OnDestroy");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "WeaponSource";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(className);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_WeaponSource");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            string BodyAnimatorClassName = default;

            {
                string name = "Player";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Player";

                {
                    entity = "_characterController";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<MovementContext>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_playerBody";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<ProceduralWeaponAnimation>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_animators";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    BodyAnimatorClassName = offset.Value.TypeName;
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Corpse";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<Location>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<InteractableObject>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<Profile>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Physical";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<AIData>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_healthController";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_inventoryController";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_handsController";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "EnabledAnimators";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<InteractionRayOriginOnStartOperation>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<InteractionRayDirectionOnStartOperation>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<IsYourPlayer>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "AIData";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                var fClass = _dnlibHelper.FindClassWithEntityName("get_FlarePower", DnlibHelper.SearchType.Method, true);

                {
                    entity = "IsAI";

                    MethodDef fMethod = _dnlibHelper.FindMethodByName(fClass, "get_IsAI");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> ObservedPlayerControllerOffset = default;

            {
                string name = "ObservedPlayerView";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.NextObservedPlayer.ObservedPlayerView";

                {
                    entity = "GroupID";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(className);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_GroupId");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "NickName";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(className);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_NickName");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "AccountId";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(className);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_AccountId");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Voice";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(className);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_Voice");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "PlayerBody";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(className);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_PlayerBody");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "ObservedPlayerController";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(className);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_ObservedPlayerController");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    ObservedPlayerControllerOffset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, ObservedPlayerControllerOffset);
                }

                {
                    entity = "Side";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(className);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_Side");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "IsAI";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(className);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_IsAI");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "VisibleToCameraType";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(className);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_VisibleToCameraType");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> ObservedPlayerStateContextOffset = default;
            DumpParser.Result<DumpParser.OffsetData> ObservedHandsControllerOffset = default;
            DumpParser.Result<DumpParser.OffsetData> InfoContainerOffset = default;
            DumpParser.Result<DumpParser.OffsetData> ObservedHealthControllerOffset = default;

            {
                string name = "ObservedPlayerController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!ObservedPlayerControllerOffset.Success)
                {
                    nestedStruct.AddOffset(name, ObservedPlayerControllerOffset);
                    goto end;
                }

                string ObservedPlayerControllerTypeName = ObservedPlayerControllerOffset.Value.TypeName.Replace("-.", "");

                {
                    entity = "Player";

                    var offset = _dumpParser.FindOffsetByTypeName(ObservedPlayerControllerTypeName, "EFT.NextObservedPlayer.ObservedPlayerView");
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "MovementController";

                    DumpParser.Result<DumpParser.OffsetData> offset1 = default;

                    TypeDef foundClass1 = _dnlibHelper.FindClassByTypeName(ObservedPlayerControllerTypeName);
                    MethodDef foundMethod1 = _dnlibHelper.FindMethodByName(foundClass1, "get_MovementController");
                    FieldDef fField1 = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod1);
                    offset1 = _dumpParser.FindOffsetByName(ObservedPlayerControllerTypeName, fField1.GetFieldName());

                    if (offset1.Success)
                    {
                        string typeName2 = offset1.Value.TypeName.Replace("-.", "");
                        TypeDef foundClass2 = _dnlibHelper.FindClassByTypeName(typeName2);
                        MethodDef foundMethod2 = _dnlibHelper.FindMethodByName(foundClass2, "get_ObservedPlayerStateContext");
                        FieldDef fField2 = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod2);
                        ObservedPlayerStateContextOffset = _dumpParser.FindOffsetByName(offset1.Value.TypeName, fField2.GetFieldName());
                    }

                    nestedStruct.AddOffsetChain(entity, new() { offset1, ObservedPlayerStateContextOffset });
                }

                {
                    entity = "HandsController";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(ObservedPlayerControllerTypeName);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_HandsController");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    ObservedHandsControllerOffset = _dumpParser.FindOffsetByName(ObservedPlayerControllerTypeName, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, ObservedHandsControllerOffset);
                }

                {
                    entity = "InfoContainer";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(ObservedPlayerControllerTypeName);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_InfoContainer");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    InfoContainerOffset = _dumpParser.FindOffsetByName(ObservedPlayerControllerTypeName, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, InfoContainerOffset);
                }

                {
                    entity = "HealthController";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(ObservedPlayerControllerTypeName);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_HealthController");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    ObservedHealthControllerOffset = _dumpParser.FindOffsetByName(ObservedPlayerControllerTypeName, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, ObservedHealthControllerOffset);
                }

                {
                    entity = "InventoryController";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(ObservedPlayerControllerTypeName);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_InventoryController");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(ObservedPlayerControllerTypeName, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ObservedMovementController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!ObservedPlayerStateContextOffset.Success)
                {
                    nestedStruct.AddOffset(name, ObservedPlayerStateContextOffset);
                    goto end;
                }

                string ObservedPlayerStateContextTypeName = ObservedPlayerStateContextOffset.Value.TypeName.Replace("-.", "");

                {
                    entity = "Rotation";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(ObservedPlayerStateContextTypeName);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_Rotation");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);
                    var offset = _dumpParser.FindOffsetByName(ObservedPlayerStateContextTypeName, fField.GetFieldName());

                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Velocity";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(ObservedPlayerStateContextTypeName);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_Velocity");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);
                    var offset = _dumpParser.FindOffsetByName(ObservedPlayerStateContextTypeName, fField.GetFieldName());

                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ObservedHandsController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!ObservedHandsControllerOffset.Success)
                {
                    nestedStruct.AddOffset(name, ObservedHandsControllerOffset);
                    goto end;
                }

                string ObservedHandsControllerTypeName = ObservedHandsControllerOffset.Value.TypeName.Replace("-.", "");

                {
                    entity = "ItemInHands";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(ObservedHandsControllerTypeName);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_ItemInHands");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);
                    var offset = _dumpParser.FindOffsetByName(ObservedHandsControllerTypeName, fField.GetFieldName());

                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ObservedHealthController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!ObservedHealthControllerOffset.Success)
                {
                    nestedStruct.AddOffset(name, ObservedHealthControllerOffset);
                    goto end;
                }

                string ObservedHealthControllerTypeName = ObservedHealthControllerOffset.Value.TypeName.Replace("-.", "");

                {
                    entity = "Player";

                    var offset = _dumpParser.FindOffsetByTypeName(ObservedHealthControllerTypeName, "EFT.NextObservedPlayer.ObservedPlayerView");
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "PlayerCorpse";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(ObservedHealthControllerTypeName);
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_PlayerCorpse");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);
                    var offset = _dumpParser.FindOffsetByName(ObservedHealthControllerTypeName, fField.GetFieldName());

                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "HealthStatus";

                    var offset = _dumpParser.FindOffsetByName(ObservedHealthControllerTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "SimpleCharacterController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "_collisionMask";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_speedLimit";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_sqrSpeedLimit";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "velocity";

                    TypeDef fClass = _dnlibHelper.FindClassByTypeName(name);
                    MethodDef fMethod = _dnlibHelper.FindMethodByName(fClass, "get_velocity");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(name, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "InfoContainer";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!InfoContainerOffset.Success)
                {
                    nestedStruct.AddOffset(name, InfoContainerOffset);
                    goto end;
                }

                string InfoContainerTypeName = InfoContainerOffset.Value.TypeName.Replace("-.", "");

                {
                    entity = "Side";

                    var offset = _dumpParser.FindOffsetByName(InfoContainerTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "PlayerSpawnInfo";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                TypeDef fClass = _dnlibHelper.FindClassWithEntityName("DeserializeCustomization", DnlibHelper.SearchType.Method);

                {
                    entity = "Side";

                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "WildSpawnType";

                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ProceduralWeaponAnimation";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Animations.ProceduralWeaponAnimation";

                {
                    entity = "HandsContainer";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_shotDirection";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Breath";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Shootingg";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Mask";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_isAiming";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_aimingSpeed";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_fovCompensatoryDistance";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_compensatoryScale";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "CameraSmoothOut";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "PositionZeroSum";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<ShotNeedsFovAdjustments>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_optics";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "MotionReact";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "SightNBone";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "ProceduralWeaponAnimation.SightNBone";

                {
                    entity = "Mod";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "MotionEffector";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "_mouseProcessors";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_movementProcessors";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "BreathEffector";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Animations.BreathEffector";

                {
                    entity = "Intensity";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ShotEffector";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "NewShotRecoil";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "NewShotRecoil";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Animations.NewRecoil.NewRecoilShotEffect";

                {
                    entity = "IntensitySeparateFactors";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "VisorEffect";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "Intensity";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> ProfileStatsOffset = default;
            DumpParser.Result<DumpParser.OffsetData> ProfileInfoOffset = default;
            DumpParser.Result<DumpParser.OffsetData> ProfileTaskConditionCountersOffset = default;
            DumpParser.Result<DumpParser.OffsetData> ProfileQuestDataOffset = default;
            DumpParser.Result<DumpParser.OffsetData> WishlistManagerOffset = default;

            {
                string name = "Profile";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Profile";

                {
                    entity = "Id";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "AccountId";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Info";

                    ProfileInfoOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, ProfileInfoOffset);
                }

                {
                    entity = "Skills";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "TaskConditionCounters";

                    ProfileTaskConditionCountersOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, ProfileTaskConditionCountersOffset);
                }

                {
                    entity = "QuestsData";

                    ProfileQuestDataOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, ProfileQuestDataOffset);
                }

                {
                    entity = "Stats";

                    ProfileStatsOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, ProfileStatsOffset);
                }

                {
                    entity = "WishlistManager";

                    WishlistManagerOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, WishlistManagerOffset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "WishlistManager";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                var fType = _dnlibHelper.FindClassByTypeName(WishlistManagerOffset.Value.TypeName.Replace("-.", ""));
                {
                    var fMethod = _dnlibHelper.FindMethodByName(fType, "get_UserItems");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);
                    entity = "Items";

                    var offset = _dumpParser.FindOffsetByName(fType.HumanizeAlt(), fField.HumanizeAlt());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> PlayerInfoSettingsOffset = default;

            {
                string name = "PlayerInfo";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!ProfileInfoOffset.Success)
                {
                    nestedStruct.AddOffset(name, ProfileInfoOffset);
                    goto end;
                }

                string ProfileInfoTypeName = ProfileInfoOffset.Value.TypeName.Replace("-.", "");

                {
                    entity = "Nickname";

                    var offset = _dumpParser.FindOffsetByName(ProfileInfoTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "GroupId";

                    var offset = _dumpParser.FindOffsetByName(ProfileInfoTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "EntryPoint";

                    var offset = _dumpParser.FindOffsetByName(ProfileInfoTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Settings";

                    var settingsClass = _dnlibHelper.FindClassWithEntityName("StandingForKill", DnlibHelper.SearchType.Field);
                    PlayerInfoSettingsOffset = _dumpParser.FindOffsetByTypeName(ProfileInfoTypeName, $"-.{settingsClass.Humanize()}");
                    nestedStruct.AddOffset(entity, PlayerInfoSettingsOffset);
                }

                var registrationDateOffset = _dumpParser.FindOffsetByName(ProfileInfoTypeName, "RegistrationDate");
                {
                    entity = "Side";

                    var offset = _dumpParser.FindOffsetByName(ProfileInfoTypeName, entity); // MANUAL OFFSET
                    nestedStruct.AddOffset(entity, new(true, new(entity, "[HUMAN] Int32", registrationDateOffset.Value.Offset - 0x4)));
                }

                {
                    entity = "RegistrationDate";
                    nestedStruct.AddOffset(entity, registrationDateOffset);
                }

                {
                    entity = "MemberCategory";

                    var offset = _dumpParser.FindOffsetByName(ProfileInfoTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Experience";

                    var fClass = _dnlibHelper.FindClassByTypeName(ProfileInfoTypeName);
                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "get_Experience");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(ProfileInfoTypeName, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "PlayerInfoSettings";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!PlayerInfoSettingsOffset.Success)
                {
                    nestedStruct.AddOffset(name, PlayerInfoSettingsOffset);
                    goto end;
                }

                string PlayerInfoSettingsTypeName = PlayerInfoSettingsOffset.Value.TypeName.Replace("-.", "");

                {
                    entity = "Role";

                    var offset = _dumpParser.FindOffsetByName(PlayerInfoSettingsTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> SkillValueContainerOffset = default;

            {
                string name = "SkillManager";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.SkillManager";

                {
                    entity = "StrengthBuffJumpHeightInc";

                    SkillValueContainerOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, SkillValueContainerOffset);
                }

                {
                    entity = "StrengthBuffThrowDistanceInc";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "MagDrillsLoadSpeed";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "MagDrillsUnloadSpeed";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "SkillValueContainer";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!SkillValueContainerOffset.Success)
                {
                    nestedStruct.AddOffset(name, SkillValueContainerOffset);
                    goto end;
                }

                string SkillValueContainerTypeName = SkillValueContainerOffset.Value.TypeName.Replace("-.", "");

                {
                    entity = "Value";

                    var offset = _dumpParser.FindOffsetByName(SkillValueContainerTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> QuestDataTemplateOffset = default;

            {
                string name = "QuestData";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!ProfileQuestDataOffset.Success)
                {
                    nestedStruct.AddOffset(name, ProfileQuestDataOffset);
                    goto end;
                }

                string ProfileQuestDataTypeName = ProfileQuestDataOffset.Value.TypeName.Split('<')[1].TrimEnd('>');

                {
                    entity = "Id";

                    var offset = _dumpParser.FindOffsetByName(ProfileQuestDataTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "CompletedConditions";

                    var offset = _dumpParser.FindOffsetByName(ProfileQuestDataTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Template";

                    QuestDataTemplateOffset = _dumpParser.FindOffsetByName(ProfileQuestDataTypeName, entity);
                    nestedStruct.AddOffset(entity, QuestDataTemplateOffset);
                }

                {
                    entity = "Status";

                    var offset = _dumpParser.FindOffsetByName(ProfileQuestDataTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> QuestTemplateOffset = default;

            {
                string name = "QuestTemplate";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!QuestDataTemplateOffset.Success)
                {
                    nestedStruct.AddOffset(name, QuestDataTemplateOffset);
                    goto end;
                }

                string QuestDataTemplateTypeName = QuestDataTemplateOffset.Value.TypeName.Replace("-.", "");
                var fClass = _dnlibHelper.FindClassByTypeName(QuestDataTemplateTypeName);

                {
                    entity = "Conditions";

                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "get_Conditions");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    QuestTemplateOffset = _dumpParser.FindOffsetByName(QuestDataTemplateTypeName, fField.GetFieldName());

                    nestedStruct.AddOffset(entity, QuestTemplateOffset);
                }

                {
                    entity = "Name";

                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "get_NameLocaleKey");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(QuestDataTemplateTypeName, fField.GetFieldName());

                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "QuestConditionsContainer";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!QuestTemplateOffset.Success)
                {
                    nestedStruct.AddOffset(name, QuestTemplateOffset);
                    goto end;
                }

                string QuestTemplateTypeName = QuestTemplateOffset.Value.TypeName.Replace("-.", "");

                {
                    entity = "ConditionsList";

                    var tClass1 = _dnlibHelper.FindClassByTypeName(QuestTemplateTypeName);
                    var typeSpec = tClass1.BaseType as TypeSpec;
                    var genericInstSig = typeSpec?.TypeSig as GenericInstSig;

                    string dictValueType = genericInstSig?.GenericArguments[1].FullName.Humanize();

                    string tClass2 = _dnlibHelper.FindClassByTypeName(dictValueType).BaseType.FullName.Humanize();
                    string tClass3 = _dnlibHelper.FindClassByTypeName(tClass2).BaseType.FullName.Humanize();

                    var fClass = _dnlibHelper.FindClassByTypeName(tClass3);
                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "get_Count");

                    string methodBody = _decompiler_Basic.DecompileClassMethod(fClass, "get_Count").Body;
                    string fField = TextHelper.FindSubstringAndGoBackwards(methodBody, ".Count", ' ');

                    var offset = _dumpParser.FindOffsetByName(tClass3, fField);

                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "QuestCondition";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Quests.Condition";

                {
                    entity = "<id>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "QuestConditionItem";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Quests.ConditionItem";

                {
                    entity = "<value>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "QuestConditionFindItem";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Quests.ConditionFindItem";

                {
                    entity = "target";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "QuestConditionCounterCreator";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Quests.ConditionCounterCreator";

                {
                    entity = "<Conditions>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "QuestConditionVisitPlace";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Quests.ConditionVisitPlace";

                {
                    entity = "target";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "QuestConditionPlaceBeacon";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Quests.ConditionPlaceBeacon";

                {
                    entity = "zoneId";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "plantTime";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "QuestConditionLaunchFlare";
                SetVariableStatus(name);
                StructureGenerator nestedStruct = new(name);
                const string className = "EFT.Quests.ConditionLaunchFlare";
                {
                    string entity = "zoneId";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "QuestConditionZone";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                // Find the ConditionZone class
                TypeDef conditionZoneClass = _dnlibHelper.FindClassByTypeName("EFT.Quests.ConditionZone");
                // Find the ConditionMultipleTargets class (base class for 'target' field)
                TypeDef conditionMultipleTargetsClass = _dnlibHelper.FindClassByTypeName("EFT.Quests.ConditionMultipleTargets");

                // Find the 'target' field (defined in ConditionMultipleTargets)
                FieldDef targetField = conditionMultipleTargetsClass.Fields.FirstOrDefault(f => f.Name == "target");
                if (targetField != null)
                {
                    // Use the full name of the defining class (ConditionMultipleTargets) and field name for the offset group
                    var targetOffset = _dumpParser.FindOffsetByName(conditionMultipleTargetsClass.FullName, targetField.Name);
                    nestedStruct.AddOffset("target", targetOffset);
                }

                // Find the 'zoneId' field (defined in ConditionZone)
                FieldDef zoneIdField = conditionZoneClass.Fields.FirstOrDefault(f => f.Name == "zoneId");
                if (zoneIdField != null)
                {
                    // Use the full name of the defining class (ConditionZone) and field name for the offset group
                    var zoneIdOffset = _dumpParser.FindOffsetByName(conditionZoneClass.FullName, zoneIdField.Name);
                    nestedStruct.AddOffset("zoneId", zoneIdOffset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "QuestConditionInZone";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                // Find the ConditionZone class
                TypeDef conditionZoneClass = _dnlibHelper.FindClassByTypeName("EFT.Quests.ConditionInZone");

                // Find the 'zoneId' field (defined in ConditionZone)
                FieldDef zoneIdField = conditionZoneClass.Fields.FirstOrDefault(f => f.Name == "zoneIds");
                if (zoneIdField != null)
                {
                    // Use the full name of the defining class (ConditionZone) and field name for the offset group
                    var zoneIdOffset = _dumpParser.FindOffsetByName(conditionZoneClass.FullName, zoneIdField.Name);
                    nestedStruct.AddOffset("zoneIds", zoneIdOffset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "QuestConditionCounterTemplate";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "ConditionCounterCreator.ConditionCounterTemplate";

                {
                    entity = "Conditions";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ItemHandsController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "Item";

                    var fClass = _dnlibHelper.FindClassByTypeName(name);
                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "GetItem");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName("Player." + name, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "FirearmController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "Player.FirearmController";

                {
                    entity = "Fireport";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "TotalCenterOfImpact";

                    var fClass = _dnlibHelper.FindClassByTypeName(className.Split('.')[1]);
                    var methodBody = _decompiler_Basic.DecompileClassMethod(fClass, "WeaponModified").Body;
                    var fField = TextHelper.FindSubstringAndGoBackwards(methodBody, " = Item.GetTotalCenterOfImpact", '.');

                    var offset = _dumpParser.FindOffsetByName(className, fField);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ClientFirearmController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.ClientFirearmController";

                {
                    entity = "WeaponLn";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "ShotIndex";

                    var fClass = _dnlibHelper.FindClassByTypeName(className);
                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "DryShot");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "MovementContext";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.MovementContext";

                {
                    entity = "Player";

                    var offset = _dumpParser.FindOffsetByTypeName(className, "EFT.Player");
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<CurrentState>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_states";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_tilt";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_rotation";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_physicalCondition";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_speedLimitIsDirty";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<StateSpeedLimit>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<StateSprintSpeedLimit>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_lookDirection";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<WalkInertia>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<SprintBrakeInertia>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_movementStates";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "MovementState";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.MovementState";

                {
                    entity = "Name";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "AnimatorStateHash";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "StickToGround";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "PlantTime";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "PlayerStateContainer";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "Name";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "StateFullNameHash";

                    var offset = _dumpParser.FindOffsetByName(name, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "InventoryController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "Player.PlayerInventoryController";

                {
                    entity = "<Inventory>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> InventoryEquipmentOffset = default;

            {
                string name = "Inventory";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.Inventory";

                {
                    entity = "Equipment";

                    InventoryEquipmentOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, InventoryEquipmentOffset);
                }

                {
                    entity = "QuestRaidItems";

                    InventoryEquipmentOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, InventoryEquipmentOffset);
                }

                {
                    entity = "QuestStashItems";

                    InventoryEquipmentOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, InventoryEquipmentOffset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            string GridsClassName = default;

            {
                string name = "Equipment";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!InventoryEquipmentOffset.Success)
                {
                    nestedStruct.AddOffset(name, InventoryEquipmentOffset);
                    goto end;
                }

                string InventoryEquipmentTypeName = InventoryEquipmentOffset.Value.TypeName.Replace("-.", "");

                {
                    entity = "Grids";

                    var offset = _dumpParser.FindOffsetByName(InventoryEquipmentTypeName, entity);
                    GridsClassName = offset.Value.TypeName.Replace("-.", "").Replace("[]", "");
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Slots";

                    var offset = _dumpParser.FindOffsetByName(InventoryEquipmentTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            string GridContainedItemsClassName = default;

            {
                string name = "Grids";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                var fClass = _dnlibHelper.FindClassByTypeName(GridsClassName);

                {
                    var fFMethod = _dnlibHelper.FindMethodByName(fClass, "get_ItemCollection");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fFMethod);
                    GridContainedItemsClassName = fField.GetTypeName();
                    entity = "ContainedItems";

                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), fField.Humanize());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "GridContainedItems";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;
                {
                    entity = "Items";

                    var offset = _dumpParser.FindOffsetByTypeName(GridContainedItemsClassName, "System.Collections.Generic.List<Item>");
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "Slot";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.Slot";

                {
                    entity = "<ID>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<ContainedItem>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Required";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "InteractiveLootItem";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Interactive.LootItem";

                {
                    entity = "Item";

                    var fClass = _dnlibHelper.FindClassByTypeName(className);
                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "get_Item");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(className, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "InteractiveCorpse";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Interactive.Corpse";

                {
                    entity = "PlayerBody";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "DizSkinningSkeleton";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "Diz.Skinning.Skeleton";

                {
                    entity = "_values";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> LootableContainerItemOwnerOffset = default;

            {
                string name = "LootableContainer";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Interactive.LootableContainer";

                {
                    entity = "ItemOwner";

                    LootableContainerItemOwnerOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, LootableContainerItemOwnerOffset);
                }

                {
                    entity = "<InteractingPlayer>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Template";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "LootableContainerItemOwner";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!LootableContainerItemOwnerOffset.Success)
                {
                    nestedStruct.AddOffset(name, LootableContainerItemOwnerOffset);
                    goto end;
                }

                string LootableContainerItemOwnerTypeName = LootableContainerItemOwnerOffset.Value.TypeName.Replace("-.", "");

                {
                    entity = "RootItem";

                    var fClass = _dnlibHelper.FindClassByTypeName(LootableContainerItemOwnerTypeName);
                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "get_RootItem");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(LootableContainerItemOwnerTypeName, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "LootItem";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.Item";

                {
                    entity = "<Template>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "StackObjectsCount";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Version";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<SpawnedInSession>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }


            {
                string name = "LootItemMod";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.Mod";

                {
                    entity = "Grids";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Slots";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "LootItemWeapon";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.Weapon";

                {
                    entity = "<Chambers>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_magSlotCache";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "FireMode";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "FireModeComponent";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.FireModeComponent";

                {
                    entity = "FireMode";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "LootItemMagazine";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                // Find the class that has all of these fields
                List<DnlibHelper.EntitySearchListEntry> searchEntities = new()
                {
                    new("GetAmmoCountByLevel", DnlibHelper.SearchType.Method),
                    new("VisibleAmmoRanges", DnlibHelper.SearchType.Property),
                };
                var magazineClassA = _dnlibHelper.FindClassWithEntities(searchEntities);
                var magazineClassB = _dnlibHelper.FindClassWithEntityName("LoadUnloadModifier", DnlibHelper.SearchType.Field);

                {
                    entity = "Cartridges";

                    var fMethod = _dnlibHelper.FindMethodByName(magazineClassA, "get_Cartridges");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(magazineClassA.Humanize(), fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "LoadUnloadModifier";

                    var offset = _dumpParser.FindOffsetByName(magazineClassB.Humanize(), entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "MagazineClass";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                var magazineClass = _dnlibHelper.FindClassWithEntityName("GetMaxVisibleAmmo", DnlibHelper.SearchType.Method);

                {
                    entity = "StackObjectsCount";

                    var offset = _dumpParser.FindOffsetByName(magazineClass.Humanize(), entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "StackSlot";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.StackSlot";

                {
                    entity = "_items";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "MaxCount";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ItemTemplate";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.ItemTemplate";

                {
                    entity = "ShortName";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "<_id>k__BackingField";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "Weight";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "QuestItem";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ModTemplate";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.ModTemplate";

                {
                    entity = "ForbidMissingVitalParts";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);

                    entity = "Velocity";
                    offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);

                    entity = "RaidModdable";
                    offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "AmmoTemplate";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.AmmoTemplate";

                {
                    entity = "InitialSpeed";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "BallisticCoeficient";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "BulletMassGram";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "BulletDiameterMilimeters";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "WeaponTemplate";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.WeaponTemplate";

                {
                    entity = "Velocity";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);

                    entity = "AllowJam";
                    offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);

                    entity = "AllowFeed";
                    offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);

                    entity = "AllowMisfire";
                    offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);

                    entity = "AllowSlide";
                    offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);

                    entity = "AllowOverheat";
                    offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> PlayerBodySlotViewsOffset = default;

            {
                string name = "PlayerBody";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.PlayerBody";

                {
                    entity = "SkeletonRootJoint";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "BodySkins";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "_bodyRenderers";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "SlotViews";

                    PlayerBodySlotViewsOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, PlayerBodySlotViewsOffset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "PlayerBodySubclass";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                var fClass = _dnlibHelper.FindClassWithEntityName("DestroyCurrentModel", DnlibHelper.SearchType.Method);

                {
                    entity = "Dresses";

                    string fixedClassName = fClass.Humanize(true).Split("EFT.")[1].Split('+')[0] + '.' + fClass.Humanize();

                    var offset = _dumpParser.FindOffsetByName(fixedClassName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "Dress";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Visual.Dress";

                {
                    entity = "Renderers";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "Skeleton";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "Diz.Skinning.Skeleton";

                {
                    entity = "_values";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "LoddedSkin";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Visual.LoddedSkin";

                {
                    entity = "_lods";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "Skin";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "Diz.Skinning.Skin";

                {
                    entity = "_skinnedMeshRenderer";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "TorsoSkin";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Visual.TorsoSkin";

                {
                    entity = "_skin";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "SlotViewsContainer";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!PlayerBodySlotViewsOffset.Success)
                {
                    nestedStruct.AddOffset(name, PlayerBodySlotViewsOffset);
                    goto end;
                }

                string PlayerBodySlotViewsTypeName = PlayerBodySlotViewsOffset.Value.TypeName.Replace("-.", "").Split('<')[0];

                {
                    entity = "Dict";

                    var offset = _dumpParser.FindOffsetByTypeName(PlayerBodySlotViewsTypeName, "System.Collections.Generic.Dictionary<Var, Var>");
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "NetworkContainer";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                var offsetGroup = _dumpParser.FindOffsetGroupWithEntities(new() { new("NextRequestIndex", DumpParser.SearchType.OffsetName) });

                {
                    entity = "NextRequestIndex";

                    var offset = _dumpParser.FindOffsetByName(offsetGroup, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "PhpSessionId";

                    var fClass = _dnlibHelper.FindClassByTypeName(offsetGroup.Replace("-.", ""));
                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "get_PhpSessionId");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(offsetGroup, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "AppVersion";

                    var fClass = _dnlibHelper.FindClassByTypeName(offsetGroup.Replace("-.", ""));
                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "get_AppVersion");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(offsetGroup, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ScreenManager";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                // Find the class that has all of these fields
                List<DnlibHelper.EntitySearchListEntry> searchEntities = new()
                {
                    new("InitChatScreen", DnlibHelper.SearchType.Method),
                };
                var screenManagerClass = _dnlibHelper.FindClassWithEntities(searchEntities);

                {
                    entity = "Instance";

                    var fMethod = _dnlibHelper.FindMethodByName(screenManagerClass, "get_Instance");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    var offset = _dumpParser.FindOffsetByName(screenManagerClass.Humanize(), fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "CurrentScreenController";

                    var baseType = screenManagerClass.BaseType.FullName.Humanize();

                    var fClass = _dnlibHelper.FindClassByTypeName(baseType);
                    string decompiledMethod = _decompiler_Basic.DecompileClassMethod(fClass, "get_CurrentScreenController").Body;
                    string fField = TextHelper.FindSubstringAndGoBackwards(decompiledMethod, ";");

                    var offset = _dumpParser.FindOffsetByName(baseType, fField);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "CurrentScreenController";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                // Find the class that has all of these fields
                List<DnlibHelper.EntitySearchListEntry> searchEntities = new()
                {
                    new("Closed", DnlibHelper.SearchType.Property),
                    new("CloseScreenForced", DnlibHelper.SearchType.Method),
                };
                var currentScreenControllerClass = _dnlibHelper.FindClassWithEntities(searchEntities);

                {
                    entity = "Generic";

                    string decompiledMethod = _decompiler_Basic.DecompileClassMethod(currentScreenControllerClass, "DisplayScreen").Body;
                    string fField = TextHelper.FindSubstringAndGoBackwards(decompiledMethod, ".Show(", '.');
                    string typeName = currentScreenControllerClass.DeclaringType.Humanize() + '.' + currentScreenControllerClass.Humanize();

                    var offset = _dumpParser.FindOffsetByName(typeName, fField);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> GetOpticCameraManagerOffset = default;

            {
                string name = "OpticCameraManagerContainer";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                TypeDef fClass = _dnlibHelper.FindClassWithEntityName("get_OpticCameraManager", DnlibHelper.SearchType.Method);

                {
                    entity = "Instance";

                    const string searchMethod = "get_Instance";
                    MethodDef fMethod = _dnlibHelper.FindMethodByName(fClass, searchMethod);
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);
                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "OpticCameraManager";

                    const string searchMethod = "get_OpticCameraManager";
                    MethodDef fMethod = _dnlibHelper.FindMethodByName(fClass, searchMethod);
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);
                    GetOpticCameraManagerOffset = _dumpParser.FindOffsetByName(fClass.Humanize(), fField.GetFieldName());
                    nestedStruct.AddOffset(entity, GetOpticCameraManagerOffset);
                }

                {
                    entity = "FPSCamera";

                    const string searchMethod = "get_Camera";
                    MethodDef fMethod = _dnlibHelper.FindMethodByName(fClass, searchMethod);
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);
                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "OpticCameraManager";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!GetOpticCameraManagerOffset.Success)
                {
                    nestedStruct.AddOffset(name, GetOpticCameraManagerOffset);
                    goto end;
                }

                string GetOpticCameraManagerTypeName = GetOpticCameraManagerOffset.Value.TypeName.Replace("-.", "");

                TypeDef fClass = _dnlibHelper.FindClassByTypeName(GetOpticCameraManagerTypeName);

                {
                    entity = "Camera";

                    const string searchMethod = "get_Camera";
                    MethodDef fMethod = _dnlibHelper.FindMethodByName(fClass, searchMethod);
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);
                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "CurrentOpticSight";

                    const string searchMethod = "get_CurrentOpticSight";
                    MethodDef fMethod = _dnlibHelper.FindMethodByName(fClass, searchMethod);
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);
                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "OpticSight";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.CameraControl.OpticSight";

                {
                    entity = "LensRenderer";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "SightComponent";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                const string className = "EFT.InventoryLogic.SightComponent";
                string entity;

                {
                    entity = "_template";

                    SkillValueContainerOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, SkillValueContainerOffset);
                }
                {
                    entity = "ScopesSelectedModes";

                    SkillValueContainerOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, SkillValueContainerOffset);
                }
                {
                    entity = "SelectedScope";

                    SkillValueContainerOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, SkillValueContainerOffset);
                }
                {
                    entity = "ScopeZoomValue";

                    SkillValueContainerOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, SkillValueContainerOffset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "SightInterface";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                var fClass = _dnlibHelper.FindClassWithEntityName("CalibrationDistances", DnlibHelper.SearchType.Field);

                {
                    entity = "Zooms";

                    var offset = _dumpParser.FindOffsetByName(fClass.Humanize(), entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }
        }

        private void ProcessEnums(StatusContext ctx, StructureGenerator structGenerator)
        {
            void SetVariableStatus(string variable)
            {
                LastStepName = variable;
                ctx.Status(variable);
            }

            {
                const string name = "EPlayerState";
                const string typeName = "EPlayerState";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                const string name = "EMemberCategory";
                const string typeName = "EMemberCategory";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields, flags: true);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                const string name = "WildSpawnType";
                const string typeName = "EFT.WildSpawnType";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                const string name = "EExfiltrationStatus";
                const string typeName = "EFT.Interactive.EExfiltrationStatus";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                const string name = "EMalfunctionState";
                const string typeName = "EMalfunctionState";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                const string name = "EPhysicalCondition";
                const string typeName = "EFT.EPhysicalCondition";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields, true);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                const string name = "EProceduralAnimationMask";
                const string typeName = "EFT.Animations.EProceduralAnimationMask";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields, true);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                const string name = "EquipmentSlot";
                const string typeName = "EquipmentSlot";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                const string name = "EFireMode";
                const string typeName = "EFireMode";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                const string name = "SynchronizableObjectType";
                const string typeName = "SynchronizableObjectType";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                const string name = "ETripwireState";
                const string typeName = "ETripwireState";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                const string name = "EQuestStatus";
                const string typeName = "EQuestStatus";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name, StructureGenerator.eStructureType.Enum);

                var eType = _dnlibHelper.FindEnumByTypeName(typeName);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields);

                structGenerator.AddStruct(nestedStruct);
            }
        }
    }
}
