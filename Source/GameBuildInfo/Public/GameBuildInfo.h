// Copyright 2025 Dream Seed LLC.

#pragma once

#include "Modules/ModuleManager.h"

class FGameBuildInfoModule : public IModuleInterface
{
public:
	virtual void StartupModule() override;
	virtual void ShutdownModule() override;
};
