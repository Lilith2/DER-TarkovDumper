using dnlib.DotNet;
using Spectre.Console;

// For Lone Arena

namespace TarkovDumper.Implementations
{
    public sealed class ArenaProcessor : Processor
    {
        private const string ASSEMBLY_INPUT_PATH = @"W:\current projects\tarky-dump\input\DLL\arena\Assembly-CSharp.dll";
        private const string DUMP_INPUT_PATH = @"W:\current projects\tarky-dump\input\arena\arena_dump.txt";
        private const string SDK_OUTPUT_PATH = @"W:\current projects\tarky-dump\output\Larena\SDK.cs";

        public ArenaProcessor() : base(ASSEMBLY_INPUT_PATH, DUMP_INPUT_PATH) { }

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
                string variable = "ClassName";
                SetVariableStatus(variable);

                StructureGenerator nestedStruct = new("FirearmController");

                var fClass = _dnlibHelper.FindClassByTypeName("FirearmController");

                nestedStruct.AddClassName(fClass, variable, "N/A", true);

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

            {
                string name = "TarkovApplication";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);
                const string ClassName = "EFT.TarkovApplication";
                string entity = "GameOperationSubclass";

                {
                    var fClass = _dnlibHelper.FindClassWithEntityName("IsInSession", DnlibHelper.SearchType.Property);
                    var offset = _dumpParser.FindOffsetByTypeName(ClassName, $"-.{fClass.Humanize()}");
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
                    entity = "LocationId";

                    TypeDef foundClass = _dnlibHelper.FindClassByTypeName("EFT.GameWorld");
                    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_LocationId");
                    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);

                    var offset = _dumpParser.FindOffsetByName(name, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, offset);
                }

                {
                    entity = "RegisteredPlayers";

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

                {
                    entity = "IsInRaid";

                    var offset = _dumpParser.FindOffsetByName(name, "SpeedLimitsEnabled"); // MANUAL OFFSET
                    nestedStruct.AddOffset(entity, new(offset.Success, new(entity, "[HUMAN] Bool", offset.Value.Offset + 0x8)));
                }

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

            {
                string name = "Player";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.Player";

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
                    entity = "Corpse";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }


                {
                    entity = "<Profile>k__BackingField";

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

                //{
                //    entity = "Player";
                //    TypeDef foundClass = _dnlibHelper.FindClassByTypeName(ObservedHealthControllerTypeName);
                //    MethodDef foundMethod = _dnlibHelper.FindMethodByName(foundClass, "get_Player"); //		\uE473.get_Player() : Player @06004CFC 				\uE473.\uE026 : Player @0400430C        where \uE026 is EFT.Player in class \uE473
                //    FieldDef fField = _dnlibHelper.GetNthFieldReferencedByMethod(foundMethod);
                //    var offset = _dumpParser.FindOffsetByName(ObservedHealthControllerTypeName, fField.GetFieldName());

                //    nestedStruct.AddOffset(entity, offset);
                //}

                {
                    entity = "HealthStatus";

                    var offset = _dumpParser.FindOffsetByName(ObservedHealthControllerTypeName, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
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

            DumpParser.Result<DumpParser.OffsetData> ProfileInfoOffset = default;
            DumpParser.Result<DumpParser.OffsetData> ProfileTaskConditionCountersOffset = default;
            DumpParser.Result<DumpParser.OffsetData> ProfileQuestDataOffset = default;

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

                structGenerator.AddStruct(nestedStruct);
            }

            /// TODO: DISABLED
            //{
            //    string name = "OverallCounters";
            //    SetVariableStatus(name);

            //    StructureGenerator nestedStruct = new(name);

            //    string entity;

            //    if (!OverallCountersOffset.Success)
            //    {
            //        nestedStruct.AddOffset(name, OverallCountersOffset);
            //        goto end;
            //    }

            //    string OverallCountersTypeName = OverallCountersOffset.Value.TypeName.Replace("-.", "");

            //    {
            //        entity = "Counters";

            //        var offset = _dumpParser.FindOffsetByName(OverallCountersTypeName, entity);
            //        nestedStruct.AddOffset(entity, offset);
            //    }

            //end:
            //    structGenerator.AddStruct(nestedStruct);
            //}

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
                    entity = "Settings";

                    var settingsClass = _dnlibHelper.FindClassWithEntityName("StandingForKill", DnlibHelper.SearchType.Field);
                    PlayerInfoSettingsOffset = _dumpParser.FindOffsetByTypeName(ProfileInfoTypeName, $"-.{settingsClass.Humanize()}");
                    nestedStruct.AddOffset(entity, PlayerInfoSettingsOffset);
                }

                var registrationDateOffset = _dumpParser.FindOffsetByName(ProfileInfoTypeName, "RegistrationDate");
                {
                    entity = "Side";

                    var offset = _dumpParser.FindOffsetByName(ProfileInfoTypeName, entity);
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
                    entity = "_rotation";
                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);

                    entity = "_player";
                    offset = _dumpParser.FindOffsetByName(className, entity);
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

                structGenerator.AddStruct(nestedStruct);
            }

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

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> LootItemModGridsOffset = default;

            {
                string name = "LootItemMod";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.Mod";

                {
                    entity = "Grids";

                    LootItemModGridsOffset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, LootItemModGridsOffset);
                }

                {
                    entity = "Slots";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> LootItemModGridsItemCollectionOffset = default;

            {
                string name = "LootItemModGrids";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!LootItemModGridsOffset.Success)
                {
                    nestedStruct.AddOffset(name, LootItemModGridsOffset);
                    goto end;
                }

                string LootItemModGridsTypeName = LootItemModGridsOffset.Value.TypeName.Replace("-.", "").Split('[')[0];

                {
                    entity = "ItemCollection";

                    var fClass = _dnlibHelper.FindClassByTypeName(LootItemModGridsTypeName);
                    var fMethod = _dnlibHelper.FindMethodByName(fClass, "get_ItemCollection");
                    var fField = _dnlibHelper.GetNthFieldReferencedByMethod(fMethod);

                    LootItemModGridsItemCollectionOffset = _dumpParser.FindOffsetByName(LootItemModGridsTypeName, fField.GetFieldName());
                    nestedStruct.AddOffset(entity, LootItemModGridsItemCollectionOffset);
                }

            end:
                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "LootItemModGridsItemCollection";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                if (!LootItemModGridsItemCollectionOffset.Success)
                {
                    nestedStruct.AddOffset(name, LootItemModGridsItemCollectionOffset);
                    goto end;
                }

                string LootItemModGridsItemCollectionTypeName = LootItemModGridsItemCollectionOffset.Value.TypeName.Replace("-.", "");

                {
                    entity = "List";

                    var offset = _dumpParser.FindOffsetByTypeName(LootItemModGridsItemCollectionTypeName, "System.Collections.Generic.List<Item>");
                    nestedStruct.AddOffset(entity, offset);
                }

            end:
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

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "ModTemplate";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                const string className = "EFT.InventoryLogic.ModTemplate";

                {
                    entity = "Velocity";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
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
                }

                structGenerator.AddStruct(nestedStruct);
            }

            DumpParser.Result<DumpParser.OffsetData> PlayerBodySlotViewsOffset = default;
            DumpParser.Result<DumpParser.OffsetData> PointOfViewOffset = default;

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

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }
                {
                    entity = "ScopesSelectedModes";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
                }
                {
                    entity = "SelectedScope";

                    var offset = _dumpParser.FindOffsetByName(className, entity);
                    nestedStruct.AddOffset(entity, offset);
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

            var NetworkGameData = _dnlibHelper.FindClassWithEntityName("IsAppropriateStatusChange", DnlibHelper.SearchType.Method);
            {
                string name = "NetworkGame";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                const string className = "EFT.EftNetworkGame";
                string entity;

                {
                    entity = "NetworkGameData";
                    var offset = _dumpParser.FindOffsetByTypeName(className, "-." + NetworkGameData.Humanize());
                    nestedStruct.AddOffset(entity, offset);
                }

                structGenerator.AddStruct(nestedStruct);
            }

            {
                string name = "NetworkGameData";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new(name);

                string entity;

                {
                    entity = "raidMode";
                    var offset = _dumpParser.FindOffsetByName(NetworkGameData.Humanize(), entity);
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
                const string name = "ERaidMode";
                const string typeName = "EFT.ERaidMode";
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
                const string name = "ColorType";
                SetVariableStatus(name);

                StructureGenerator nestedStruct = new("ArmbandColorType", StructureGenerator.eStructureType.Enum);

                var fClass = _dnlibHelper.FindClassWithEntityName("GetTeamDarkColor", DnlibHelper.SearchType.Method);
                var eType = _dnlibHelper.FindEnumByTypeName(fClass.FullName + "." + name);
                var eFields = _dnlibHelper.GetEnumValues(eType);

                nestedStruct.AddEnum(eFields);

                structGenerator.AddStruct(nestedStruct);
            }

            {
                const string name = "ECameraType";
                const string typeName = "EFT.CameraControl.ECameraType";
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
