﻿// Copyright (C) 2022 Dennis Tang. All rights reserved.
//
// This file is part of WeekendBot.
//
// WeekendBot is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using WeekendBot.Utils;

namespace Discord.Common.InfoModule
{
    /// <summary>
    /// Definition of commands that provide information about the bot.
    /// </summary>
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        private readonly CommandService commandService;
        private readonly BotInformationService botInformationService;

        /// <summary>
        /// Creates a new instance of <see cref="InfoModule"/>.
        /// </summary>
        /// <param name="commandService">The <see cref="CommandService"/>.</param>
        /// <param name="botInformationService">The <see cref="BotInformationService"/>.</param>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is <c>null</c>.</exception>
        public InfoModule(CommandService commandService, BotInformationService botInformationService)
        {
            commandService.IsNotNull(nameof(commandService));
            botInformationService.IsNotNull(nameof(botInformationService));

            this.commandService = commandService;
            this.botInformationService = botInformationService;
        }

        [Command("help")]
        [Summary("Provides information about all the available commands.")]
        public async Task GetHelpResponseAsync()
        {
            IEnumerable<DiscordCommandInformation> commandInfos =
                commandService.Commands.Select(c => new DiscordCommandInformation(c.Name)
                {
                    Summary = c.Summary
                }).ToArray();

            Embed embedSummaryInformation = await botInformationService.GetCommandInfoSummaries(commandInfos);
            await ReplyAsync(null, false, embedSummaryInformation);
        }
    }
}