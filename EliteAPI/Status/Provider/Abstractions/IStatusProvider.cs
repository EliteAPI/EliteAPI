﻿using System.IO;
using System.Threading.Tasks;

namespace EliteAPI.Status.Provider.Abstractions
{
    /// <summary>
    /// Provides status files
    /// </summary>
    public interface IStatusProvider
    {
        /// <summary>
        /// Finds the status file from the specified journal directory
        /// </summary>
        /// <param name="journalDirectory"> The journal directory </param>
        Task<FileInfo> FindStatusFile(DirectoryInfo journalDirectory);

        /// <summary>
        /// Finds the market file from the specified journal directory
        /// </summary>
        /// <param name="journalDirectory"> The journal directory </param>
        Task<FileInfo> FindMarketFile(DirectoryInfo journalDirectory);

        /// <summary>
        /// Finds the cargo file from the specified journal directory
        /// </summary>
        /// <param name="journalDirectory"> The journal directory </param>
        Task<FileInfo> FindCargoFile(DirectoryInfo journalDirectory);

        /// <summary>
        /// Finds the shipyard file from the specified journal directory
        /// </summary>
        /// <param name="journalDirectory"> The journal directory </param>
        Task<FileInfo> FindShipyardFile(DirectoryInfo journalDirectory);

        /// <summary>
        /// Finds the outfitting file from the specified journal directory
        /// </summary>
        /// <param name="journalDirectory"> The journal directory </param>
        Task<FileInfo> FindOutfittingFile(DirectoryInfo journalDirectory);

        /// <summary>
        /// Finds the modules file from the specified journal directory
        /// </summary>
        /// <param name="journalDirectory"> The journal directory </param>
        Task<FileInfo> FindModulesFile(DirectoryInfo journalDirectory);

        /// <summary>
        /// Finds the navroute file from the specified journal directory
        /// </summary>
        /// <param name="journalDirectory"> The journal directory </param>
        Task<FileInfo> FindNavRouteFile(DirectoryInfo journalDirectory);

        /// <summary>
        /// Finds the backpack file from the specified journal directory
        /// </summary>
        /// <param name="journalDirectory"> The journal directory </param>
        Task<FileInfo> FindBackpackFile(DirectoryInfo journalDirectory);
    }
}