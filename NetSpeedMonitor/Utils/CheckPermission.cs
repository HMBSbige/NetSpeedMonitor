using System;
using System.Runtime.InteropServices;

namespace NetSpeedMonitor.Utils
{
	public static class CheckPermission
	{
		#region API

		/// <summary>
		/// The AllocateAndInitializeSid function allocates and initializes a security identifier (SID) with up to eight subauthorities.
		/// </summary>
		/// <param name="pIdentifierAuthority">Pointer to a SID_IDENTIFIER_AUTHORITY structure, giving the top-level identifier authority value to set in the SID.</param>
		/// <param name="nSubAuthorityCount">Specifies the number of subauthorities to place in the SID. This parameter also identifies how many of the subauthority parameters have meaningful values. This parameter must contain a value from 1 to 8.</param>
		/// <param name="dwSubAuthority0">Subauthority value to place in the SID.</param>
		/// <param name="dwSubAuthority1">Subauthority value to place in the SID.</param>
		/// <param name="dwSubAuthority2">Subauthority value to place in the SID.</param>
		/// <param name="dwSubAuthority3">Subauthority value to place in the SID.</param>
		/// <param name="dwSubAuthority4">Subauthority value to place in the SID.</param>
		/// <param name="dwSubAuthority5">Subauthority value to place in the SID.</param>
		/// <param name="dwSubAuthority6">Subauthority value to place in the SID.</param>
		/// <param name="dwSubAuthority7">Subauthority value to place in the SID.</param>
		/// <param name="pSid">Pointer to a variable that receives the pointer to the allocated and initialized SID structure.</param>
		/// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call GetLastError.</returns>
		[DllImport(@"advapi32.dll")]
		private static extern int AllocateAndInitializeSid(byte[] pIdentifierAuthority, byte nSubAuthorityCount, int dwSubAuthority0, int dwSubAuthority1, int dwSubAuthority2, int dwSubAuthority3, int dwSubAuthority4, int dwSubAuthority5, int dwSubAuthority6, int dwSubAuthority7, out IntPtr pSid);
		/// <summary>
		/// The CheckTokenMembership function determines whether a specified SID is enabled in an access token.
		/// </summary>
		/// <param name="TokenHandle">Handle to an access token. The handle must have TOKEN_QUERY access to the token. The token must be an impersonation token.</param>
		/// <param name="SidToCheck">Pointer to a SID structure. The CheckTokenMembership function checks for the presence of this SID in the user and group SIDs of the access token.</param>
		/// <param name="IsMember">Pointer to a variable that receives the results of the check. If the SID is present and has the SE_GROUP_ENABLED attribute, IsMember returns TRUE; otherwise, it returns FALSE.</param>
		/// <returns>If the function succeeds, the return value is nonzero. If the function fails, the return value is zero. To get extended error information, call GetLastError.</returns>
		[DllImport(@"advapi32.dll")]
		private static extern int CheckTokenMembership(IntPtr TokenHandle, IntPtr SidToCheck, ref int IsMember);
		/// <summary>
		/// The FreeSid function frees a security identifier (SID) previously allocated by using the AllocateAndInitializeSid function.
		/// </summary>
		/// <param name="pSid">Pointer to the SID structure to free.</param>
		/// <returns>This function does not return a value.</returns>
		[DllImport(@"advapi32.dll")]
		private static extern IntPtr FreeSid(IntPtr pSid);

		#endregion

		public static bool IsAdmin()
		{
			const int SECURITY_BUILTIN_DOMAIN_RID = 0x20;
			const int DOMAIN_ALIAS_RID_ADMINS = 0x220;

			var NtAuthority = new byte[6];
			NtAuthority[5] = 5; // SECURITY_NT_AUTHORITY
			var ret = AllocateAndInitializeSid(NtAuthority, 2, SECURITY_BUILTIN_DOMAIN_RID, DOMAIN_ALIAS_RID_ADMINS, 0, 0, 0, 0, 0, 0, out var administratorsGroup);
			if (ret != 0)
			{
				if (CheckTokenMembership(IntPtr.Zero, administratorsGroup, ref ret) == 0)
				{
					ret = 0;
				}

				FreeSid(administratorsGroup);
			}

			return ret != 0;
		}
	}
}
