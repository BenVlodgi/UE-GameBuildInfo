// Copyright 2025 Dream Seed LLC.

#include "GameBuildInfoLibrary.h"


#include "GameBuildInfo.h"

UGameBuildInfoLibrary::UGameBuildInfoLibrary(const FObjectInitializer& ObjectInitializer)
: Super(ObjectInitializer)
{

}

void UGameBuildInfoLibrary::BuildDateUTC(FDateTime& BuildDateUTC)
{
	FString BuildDateString(BUILD_DATETIME_UTC);
	BuildDateString.RemoveFromEnd("Z");
	FDateTime::ParseIso8601(*BuildDateString, BuildDateUTC);
}

bool UGameBuildInfoLibrary::BuildRevision(int& Revision)
{
#ifdef BUILD_P4_REVISION
	Revision = FCString::Atoi(TEXT(BUILD_P4_REVISION));
	return Revision >= 0;
#else
	Revision = -1;
	return false;
#endif
}
