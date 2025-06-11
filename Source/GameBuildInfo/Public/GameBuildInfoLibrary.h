// Copyright 2025 Dream Seed LLC.

#pragma once

#include "Kismet/BlueprintFunctionLibrary.h"


#include "GameBuildInfoLibrary.generated.h"


UCLASS()
class UGameBuildInfoLibrary : public UBlueprintFunctionLibrary
{
	GENERATED_UCLASS_BODY()
	
public:
	// Build DateTime, for displaying to end users in diagnostics.
	UFUNCTION(BlueprintPure, Category = "Development", meta = (BlueprintThreadSafe))
	static void BuildDateUTC(FDateTime& BuildDateUTC);

	// Build Source Control Revision, for displaying to end users in diagnostics.
	UFUNCTION(BlueprintPure, Category = "Development", meta = (BlueprintThreadSafe))
	static bool BuildRevision(int& Revision);

};
