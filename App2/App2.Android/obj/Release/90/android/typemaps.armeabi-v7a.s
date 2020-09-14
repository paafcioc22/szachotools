	.arch	armv7-a
	.syntax unified
	.eabi_attribute 67, "2.09"	@ Tag_conformance
	.eabi_attribute 6, 10	@ Tag_CPU_arch
	.eabi_attribute 7, 65	@ Tag_CPU_arch_profile
	.eabi_attribute 8, 1	@ Tag_ARM_ISA_use
	.eabi_attribute 9, 2	@ Tag_THUMB_ISA_use
	.fpu	vfpv3-d16
	.eabi_attribute 34, 1	@ Tag_CPU_unaligned_access
	.eabi_attribute 15, 1	@ Tag_ABI_PCS_RW_data
	.eabi_attribute 16, 1	@ Tag_ABI_PCS_RO_data
	.eabi_attribute 17, 2	@ Tag_ABI_PCS_GOT_use
	.eabi_attribute 20, 2	@ Tag_ABI_FP_denormal
	.eabi_attribute 21, 0	@ Tag_ABI_FP_exceptions
	.eabi_attribute 23, 3	@ Tag_ABI_FP_number_model
	.eabi_attribute 24, 1	@ Tag_ABI_align_needed
	.eabi_attribute 25, 1	@ Tag_ABI_align_preserved
	.eabi_attribute 38, 1	@ Tag_ABI_FP_16bit_format
	.eabi_attribute 18, 4	@ Tag_ABI_PCS_wchar_t
	.eabi_attribute 26, 2	@ Tag_ABI_enum_size
	.eabi_attribute 14, 0	@ Tag_ABI_PCS_R9_use
	.file	"typemaps.armeabi-v7a.s"

/* map_module_count: START */
	.section	.rodata.map_module_count,"a",%progbits
	.type	map_module_count, %object
	.p2align	2
	.global	map_module_count
map_module_count:
	.size	map_module_count, 4
	.long	26
/* map_module_count: END */

/* java_type_count: START */
	.section	.rodata.java_type_count,"a",%progbits
	.type	java_type_count, %object
	.p2align	2
	.global	java_type_count
java_type_count:
	.size	java_type_count, 4
	.long	977
/* java_type_count: END */

/* java_name_width: START */
	.section	.rodata.java_name_width,"a",%progbits
	.type	java_name_width, %object
	.p2align	2
	.global	java_name_width
java_name_width:
	.size	java_name_width, 4
	.long	102
/* java_name_width: END */

	.include	"typemaps.armeabi-v7a-shared.inc"
	.include	"typemaps.armeabi-v7a-managed.inc"

/* Managed to Java map: START */
	.section	.data.rel.map_modules,"aw",%progbits
	.type	map_modules, %object
	.p2align	2
	.global	map_modules
map_modules:
	/* module_uuid: 5022870b-5e38-4b59-a57f-d911e6b24b7c */
	.byte	0x0b, 0x87, 0x22, 0x50, 0x38, 0x5e, 0x59, 0x4b, 0xa5, 0x7f, 0xd9, 0x11, 0xe6, 0xb2, 0x4b, 0x7c
	/* entry_count */
	.long	5
	/* duplicate_count */
	.long	1
	/* map */
	.long	module0_managed_to_java
	/* duplicate_map */
	.long	module0_managed_to_java_duplicates
	/* assembly_name: Xamarin.Android.Support.Loader */
	.long	.L.map_aname.0
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 822b9615-d63e-4628-be9d-53a9db50b9ef */
	.byte	0x15, 0x96, 0x2b, 0x82, 0x3e, 0xd6, 0x28, 0x46, 0xbe, 0x9d, 0x53, 0xa9, 0xdb, 0x50, 0xb9, 0xef
	/* entry_count */
	.long	180
	/* duplicate_count */
	.long	0
	/* map */
	.long	module1_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: Xamarin.Forms.Platform.Android */
	.long	.L.map_aname.1
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 035c591e-94e1-4b42-b4f8-519744aa635f */
	.byte	0x1e, 0x59, 0x5c, 0x03, 0xe1, 0x94, 0x42, 0x4b, 0xb4, 0xf8, 0x51, 0x97, 0x44, 0xaa, 0x63, 0x5f
	/* entry_count */
	.long	43
	/* duplicate_count */
	.long	14
	/* map */
	.long	module2_managed_to_java
	/* duplicate_map */
	.long	module2_managed_to_java_duplicates
	/* assembly_name: Xamarin.Android.Support.v7.RecyclerView */
	.long	.L.map_aname.2
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: b398c227-effe-4da3-8837-c31e3e66b8ac */
	.byte	0x27, 0xc2, 0x98, 0xb3, 0xfe, 0xef, 0xa3, 0x4d, 0x88, 0x37, 0xc3, 0x1e, 0x3e, 0x66, 0xb8, 0xac
	/* entry_count */
	.long	1
	/* duplicate_count */
	.long	0
	/* map */
	.long	module3_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: Xamarin.Essentials */
	.long	.L.map_aname.3
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: fecae327-b4e7-4fb4-8aa4-c73963b3eef3 */
	.byte	0x27, 0xe3, 0xca, 0xfe, 0xe7, 0xb4, 0xb4, 0x4f, 0x8a, 0xa4, 0xc7, 0x39, 0x63, 0xb3, 0xee, 0xf3
	/* entry_count */
	.long	21
	/* duplicate_count */
	.long	1
	/* map */
	.long	module4_managed_to_java
	/* duplicate_map */
	.long	module4_managed_to_java_duplicates
	/* assembly_name: Xamarin.Android.Support.Design */
	.long	.L.map_aname.4
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 5b9fb342-0ac7-4db5-9164-ef27b805719d */
	.byte	0x42, 0xb3, 0x9f, 0x5b, 0xc7, 0x0a, 0xb5, 0x4d, 0x91, 0x64, 0xef, 0x27, 0xb8, 0x05, 0x71, 0x9d
	/* entry_count */
	.long	491
	/* duplicate_count */
	.long	81
	/* map */
	.long	module5_managed_to_java
	/* duplicate_map */
	.long	module5_managed_to_java_duplicates
	/* assembly_name: Mono.Android */
	.long	.L.map_aname.5
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 2e78a871-8b12-48b5-8c88-7912dacc1ae5 */
	.byte	0x71, 0xa8, 0x78, 0x2e, 0x12, 0x8b, 0xb5, 0x48, 0x8c, 0x88, 0x79, 0x12, 0xda, 0xcc, 0x1a, 0xe5
	/* entry_count */
	.long	3
	/* duplicate_count */
	.long	0
	/* map */
	.long	module6_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: FFImageLoading.Forms.Platform */
	.long	.L.map_aname.6
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: c5c40578-35bc-4ae4-87b0-0849f21bd82a */
	.byte	0x78, 0x05, 0xc4, 0xc5, 0xbc, 0x35, 0xe4, 0x4a, 0x87, 0xb0, 0x08, 0x49, 0xf2, 0x1b, 0xd8, 0x2a
	/* entry_count */
	.long	2
	/* duplicate_count */
	.long	0
	/* map */
	.long	module7_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: FormsViewGroup */
	.long	.L.map_aname.7
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: e4a68679-a2a4-4c46-94e0-4140bb609f9a */
	.byte	0x79, 0x86, 0xa6, 0xe4, 0xa4, 0xa2, 0x46, 0x4c, 0x94, 0xe0, 0x41, 0x40, 0xbb, 0x60, 0x9f, 0x9a
	/* entry_count */
	.long	6
	/* duplicate_count */
	.long	0
	/* map */
	.long	module8_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: FFImageLoading.Platform */
	.long	.L.map_aname.8
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 9f646a7d-0691-41bb-922b-3d2db31483e1 */
	.byte	0x7d, 0x6a, 0x64, 0x9f, 0x91, 0x06, 0xbb, 0x41, 0x92, 0x2b, 0x3d, 0x2d, 0xb3, 0x14, 0x83, 0xe1
	/* entry_count */
	.long	1
	/* duplicate_count */
	.long	0
	/* map */
	.long	module9_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: FastAndroidCamera */
	.long	.L.map_aname.9
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: f16c5186-7b23-46eb-9f75-61917da86866 */
	.byte	0x86, 0x51, 0x6c, 0xf1, 0x23, 0x7b, 0xeb, 0x46, 0x9f, 0x75, 0x61, 0x91, 0x7d, 0xa8, 0x68, 0x66
	/* entry_count */
	.long	3
	/* duplicate_count */
	.long	1
	/* map */
	.long	module10_managed_to_java
	/* duplicate_map */
	.long	module10_managed_to_java_duplicates
	/* assembly_name: Xamarin.Android.Support.CoordinaterLayout */
	.long	.L.map_aname.10
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 986c0a98-64dc-494a-a5a8-8a170b28f82f */
	.byte	0x98, 0x0a, 0x6c, 0x98, 0xdc, 0x64, 0x4a, 0x49, 0xa5, 0xa8, 0x8a, 0x17, 0x0b, 0x28, 0xf8, 0x2f
	/* entry_count */
	.long	4
	/* duplicate_count */
	.long	0
	/* map */
	.long	module11_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: Xamarin.Android.Support.DrawerLayout */
	.long	.L.map_aname.11
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 6b1529a5-88a9-46a4-a656-d22cbe194ee5 */
	.byte	0xa5, 0x29, 0x15, 0x6b, 0xa9, 0x88, 0xa4, 0x46, 0xa6, 0x56, 0xd2, 0x2c, 0xbe, 0x19, 0x4e, 0xe5
	/* entry_count */
	.long	83
	/* duplicate_count */
	.long	10
	/* map */
	.long	module12_managed_to_java
	/* duplicate_map */
	.long	module12_managed_to_java_duplicates
	/* assembly_name: Xamarin.Sewoo.AndroidSDK */
	.long	.L.map_aname.12
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 819b29aa-6d91-4626-99e1-9a679be55a44 */
	.byte	0xaa, 0x29, 0x9b, 0x81, 0x91, 0x6d, 0x26, 0x46, 0x99, 0xe1, 0x9a, 0x67, 0x9b, 0xe5, 0x5a, 0x44
	/* entry_count */
	.long	6
	/* duplicate_count */
	.long	0
	/* map */
	.long	module13_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: ZXingNetMobile */
	.long	.L.map_aname.13
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 88c944ac-f901-418b-9612-82e0b4eff328 */
	.byte	0xac, 0x44, 0xc9, 0x88, 0x01, 0xf9, 0x8b, 0x41, 0x96, 0x12, 0x82, 0xe0, 0xb4, 0xef, 0xf3, 0x28
	/* entry_count */
	.long	1
	/* duplicate_count */
	.long	0
	/* map */
	.long	module14_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: Xamarin.Android.Support.v7.CardView */
	.long	.L.map_aname.14
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 9741ffb1-34fe-4933-b80d-0d480c3aab5d */
	.byte	0xb1, 0xff, 0x41, 0x97, 0xfe, 0x34, 0x33, 0x49, 0xb8, 0x0d, 0x0d, 0x48, 0x0c, 0x3a, 0xab, 0x5d
	/* entry_count */
	.long	54
	/* duplicate_count */
	.long	2
	/* map */
	.long	module15_managed_to_java
	/* duplicate_map */
	.long	module15_managed_to_java_duplicates
	/* assembly_name: Xamarin.Android.Support.Compat */
	.long	.L.map_aname.15
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 4f0d1fcd-9704-41d1-852f-a165d24003c9 */
	.byte	0xcd, 0x1f, 0x0d, 0x4f, 0x04, 0x97, 0xd1, 0x41, 0x85, 0x2f, 0xa1, 0x65, 0xd2, 0x40, 0x03, 0xc9
	/* entry_count */
	.long	10
	/* duplicate_count */
	.long	4
	/* map */
	.long	module16_managed_to_java
	/* duplicate_map */
	.long	module16_managed_to_java_duplicates
	/* assembly_name: Xamarin.Android.Support.Fragment */
	.long	.L.map_aname.16
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 98cbbacd-7d3d-433c-b0c4-3f5cdc4576b2 */
	.byte	0xcd, 0xba, 0xcb, 0x98, 0x3d, 0x7d, 0x3c, 0x43, 0xb0, 0xc4, 0x3f, 0x5c, 0xdc, 0x45, 0x76, 0xb2
	/* entry_count */
	.long	2
	/* duplicate_count */
	.long	1
	/* map */
	.long	module17_managed_to_java
	/* duplicate_map */
	.long	module17_managed_to_java_duplicates
	/* assembly_name: Xamarin.Android.Arch.Lifecycle.LiveData.Core */
	.long	.L.map_aname.17
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 6a1930ce-ea03-4eb2-8eda-2834838f88c6 */
	.byte	0xce, 0x30, 0x19, 0x6a, 0x03, 0xea, 0xb2, 0x4e, 0x8e, 0xda, 0x28, 0x34, 0x83, 0x8f, 0x88, 0xc6
	/* entry_count */
	.long	2
	/* duplicate_count */
	.long	0
	/* map */
	.long	module18_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: ZXing.Net.Mobile.Forms.Android */
	.long	.L.map_aname.18
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 78573dd6-9a38-452f-9c19-e23dcb2ce587 */
	.byte	0xd6, 0x3d, 0x57, 0x78, 0x38, 0x9a, 0x2f, 0x45, 0x9c, 0x19, 0xe2, 0x3d, 0xcb, 0x2c, 0xe5, 0x87
	/* entry_count */
	.long	2
	/* duplicate_count */
	.long	0
	/* map */
	.long	module19_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: Xamarin.Android.Arch.Lifecycle.ViewModel */
	.long	.L.map_aname.19
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: e80064d7-1479-4e18-9b39-e4f46457da46 */
	.byte	0xd7, 0x64, 0x00, 0xe8, 0x79, 0x14, 0x18, 0x4e, 0x9b, 0x39, 0xe4, 0xf4, 0x64, 0x57, 0xda, 0x46
	/* entry_count */
	.long	1
	/* duplicate_count */
	.long	0
	/* map */
	.long	module20_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: App2.Android */
	.long	.L.map_aname.20
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 619091d8-eba4-439a-becb-327b497e7c98 */
	.byte	0xd8, 0x91, 0x90, 0x61, 0xa4, 0xeb, 0x9a, 0x43, 0xbe, 0xcb, 0x32, 0x7b, 0x49, 0x7e, 0x7c, 0x98
	/* entry_count */
	.long	4
	/* duplicate_count */
	.long	1
	/* map */
	.long	module21_managed_to_java
	/* duplicate_map */
	.long	module21_managed_to_java_duplicates
	/* assembly_name: Xamarin.Android.Arch.Lifecycle.Common */
	.long	.L.map_aname.21
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 015a84de-4015-478c-bfac-5513663b29ea */
	.byte	0xde, 0x84, 0x5a, 0x01, 0x15, 0x40, 0x8c, 0x47, 0xbf, 0xac, 0x55, 0x13, 0x66, 0x3b, 0x29, 0xea
	/* entry_count */
	.long	40
	/* duplicate_count */
	.long	4
	/* map */
	.long	module22_managed_to_java
	/* duplicate_map */
	.long	module22_managed_to_java_duplicates
	/* assembly_name: Xamarin.Android.Support.v7.AppCompat */
	.long	.L.map_aname.22
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: 3f8ed5de-79a6-4ed8-8876-9ec4238a40fc */
	.byte	0xde, 0xd5, 0x8e, 0x3f, 0xa6, 0x79, 0xd8, 0x4e, 0x88, 0x76, 0x9e, 0xc4, 0x23, 0x8a, 0x40, 0xfc
	/* entry_count */
	.long	7
	/* duplicate_count */
	.long	1
	/* map */
	.long	module23_managed_to_java
	/* duplicate_map */
	.long	module23_managed_to_java_duplicates
	/* assembly_name: Xamarin.Android.Support.ViewPager */
	.long	.L.map_aname.23
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: c51afbec-330a-4259-9761-c87a1a40aece */
	.byte	0xec, 0xfb, 0x1a, 0xc5, 0x0a, 0x33, 0x59, 0x42, 0x97, 0x61, 0xc8, 0x7a, 0x1a, 0x40, 0xae, 0xce
	/* entry_count */
	.long	1
	/* duplicate_count */
	.long	0
	/* map */
	.long	module24_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: Xamarin.Android.Support.Core.UI */
	.long	.L.map_aname.24
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	/* module_uuid: a7ffdbf4-7477-4d2e-9f15-d59292e03962 */
	.byte	0xf4, 0xdb, 0xff, 0xa7, 0x77, 0x74, 0x2e, 0x4d, 0x9f, 0x15, 0xd5, 0x92, 0x92, 0xe0, 0x39, 0x62
	/* entry_count */
	.long	4
	/* duplicate_count */
	.long	0
	/* map */
	.long	module25_managed_to_java
	/* duplicate_map */
	.long	0
	/* assembly_name: Xamarin.Android.Support.SwipeRefreshLayout */
	.long	.L.map_aname.25
	/* image */
	.long	0
	/* java_name_width */
	.long	0
	/* java_map */
	.long	0

	.size	map_modules, 1248
/* Managed to Java map: END */

/* Java to managed map: START */
	.section	.rodata.map_java,"a",%progbits
	.type	map_java, %object
	.p2align	2
	.global	map_java
map_java:
	/* #0 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554986
	/* java_name */
	.ascii	"android/animation/Animator"
	.zero	76

	/* #1 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554988
	/* java_name */
	.ascii	"android/animation/Animator$AnimatorListener"
	.zero	59

	/* #2 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554990
	/* java_name */
	.ascii	"android/animation/Animator$AnimatorPauseListener"
	.zero	54

	/* #3 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555000
	/* java_name */
	.ascii	"android/animation/AnimatorListenerAdapter"
	.zero	61

	/* #4 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555003
	/* java_name */
	.ascii	"android/animation/TimeInterpolator"
	.zero	68

	/* #5 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554992
	/* java_name */
	.ascii	"android/animation/ValueAnimator"
	.zero	71

	/* #6 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554994
	/* java_name */
	.ascii	"android/animation/ValueAnimator$AnimatorUpdateListener"
	.zero	48

	/* #7 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555005
	/* java_name */
	.ascii	"android/app/ActionBar"
	.zero	81

	/* #8 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555007
	/* java_name */
	.ascii	"android/app/ActionBar$Tab"
	.zero	77

	/* #9 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555010
	/* java_name */
	.ascii	"android/app/ActionBar$TabListener"
	.zero	69

	/* #10 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555012
	/* java_name */
	.ascii	"android/app/Activity"
	.zero	82

	/* #11 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555013
	/* java_name */
	.ascii	"android/app/ActivityManager"
	.zero	75

	/* #12 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555014
	/* java_name */
	.ascii	"android/app/ActivityManager$MemoryInfo"
	.zero	64

	/* #13 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555015
	/* java_name */
	.ascii	"android/app/AlertDialog"
	.zero	79

	/* #14 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555016
	/* java_name */
	.ascii	"android/app/AlertDialog$Builder"
	.zero	71

	/* #15 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555017
	/* java_name */
	.ascii	"android/app/Application"
	.zero	79

	/* #16 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555019
	/* java_name */
	.ascii	"android/app/Application$ActivityLifecycleCallbacks"
	.zero	52

	/* #17 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555020
	/* java_name */
	.ascii	"android/app/DatePickerDialog"
	.zero	74

	/* #18 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555023
	/* java_name */
	.ascii	"android/app/DatePickerDialog$OnDateSetListener"
	.zero	56

	/* #19 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555025
	/* java_name */
	.ascii	"android/app/Dialog"
	.zero	84

	/* #20 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555040
	/* java_name */
	.ascii	"android/app/Fragment"
	.zero	82

	/* #21 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555041
	/* java_name */
	.ascii	"android/app/FragmentTransaction"
	.zero	71

	/* #22 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555043
	/* java_name */
	.ascii	"android/app/PendingIntent"
	.zero	77

	/* #23 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555033
	/* java_name */
	.ascii	"android/app/TimePickerDialog"
	.zero	74

	/* #24 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555035
	/* java_name */
	.ascii	"android/app/TimePickerDialog$OnTimeSetListener"
	.zero	56

	/* #25 */
	/* module_index */
	.long	21
	/* type_token_id */
	.long	33554435
	/* java_name */
	.ascii	"android/arch/lifecycle/Lifecycle"
	.zero	70

	/* #26 */
	/* module_index */
	.long	21
	/* type_token_id */
	.long	33554436
	/* java_name */
	.ascii	"android/arch/lifecycle/Lifecycle$State"
	.zero	64

	/* #27 */
	/* module_index */
	.long	21
	/* type_token_id */
	.long	33554438
	/* java_name */
	.ascii	"android/arch/lifecycle/LifecycleObserver"
	.zero	62

	/* #28 */
	/* module_index */
	.long	21
	/* type_token_id */
	.long	33554440
	/* java_name */
	.ascii	"android/arch/lifecycle/LifecycleOwner"
	.zero	65

	/* #29 */
	/* module_index */
	.long	17
	/* type_token_id */
	.long	33554437
	/* java_name */
	.ascii	"android/arch/lifecycle/LiveData"
	.zero	71

	/* #30 */
	/* module_index */
	.long	17
	/* type_token_id */
	.long	33554436
	/* java_name */
	.ascii	"android/arch/lifecycle/Observer"
	.zero	71

	/* #31 */
	/* module_index */
	.long	19
	/* type_token_id */
	.long	33554437
	/* java_name */
	.ascii	"android/arch/lifecycle/ViewModelStore"
	.zero	65

	/* #32 */
	/* module_index */
	.long	19
	/* type_token_id */
	.long	33554436
	/* java_name */
	.ascii	"android/arch/lifecycle/ViewModelStoreOwner"
	.zero	60

	/* #33 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554984
	/* java_name */
	.ascii	"android/bluetooth/BluetoothAdapter"
	.zero	68

	/* #34 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554985
	/* java_name */
	.ascii	"android/bluetooth/BluetoothDevice"
	.zero	69

	/* #35 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555049
	/* java_name */
	.ascii	"android/content/ActivityNotFoundException"
	.zero	61

	/* #36 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555050
	/* java_name */
	.ascii	"android/content/BroadcastReceiver"
	.zero	69

	/* #37 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555053
	/* java_name */
	.ascii	"android/content/ClipData"
	.zero	78

	/* #38 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555052
	/* java_name */
	.ascii	"android/content/ClipboardManager"
	.zero	70

	/* #39 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555061
	/* java_name */
	.ascii	"android/content/ComponentCallbacks"
	.zero	68

	/* #40 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555063
	/* java_name */
	.ascii	"android/content/ComponentCallbacks2"
	.zero	67

	/* #41 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555054
	/* java_name */
	.ascii	"android/content/ComponentName"
	.zero	73

	/* #42 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555055
	/* java_name */
	.ascii	"android/content/ContentResolver"
	.zero	71

	/* #43 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555046
	/* java_name */
	.ascii	"android/content/Context"
	.zero	79

	/* #44 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555058
	/* java_name */
	.ascii	"android/content/ContextWrapper"
	.zero	72

	/* #45 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555075
	/* java_name */
	.ascii	"android/content/DialogInterface"
	.zero	71

	/* #46 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555065
	/* java_name */
	.ascii	"android/content/DialogInterface$OnCancelListener"
	.zero	54

	/* #47 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555068
	/* java_name */
	.ascii	"android/content/DialogInterface$OnClickListener"
	.zero	55

	/* #48 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555072
	/* java_name */
	.ascii	"android/content/DialogInterface$OnDismissListener"
	.zero	53

	/* #49 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555047
	/* java_name */
	.ascii	"android/content/Intent"
	.zero	80

	/* #50 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555076
	/* java_name */
	.ascii	"android/content/IntentFilter"
	.zero	74

	/* #51 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555077
	/* java_name */
	.ascii	"android/content/IntentSender"
	.zero	74

	/* #52 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555083
	/* java_name */
	.ascii	"android/content/SharedPreferences"
	.zero	69

	/* #53 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555079
	/* java_name */
	.ascii	"android/content/SharedPreferences$Editor"
	.zero	62

	/* #54 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555081
	/* java_name */
	.ascii	"android/content/SharedPreferences$OnSharedPreferenceChangeListener"
	.zero	36

	/* #55 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555085
	/* java_name */
	.ascii	"android/content/pm/ApplicationInfo"
	.zero	68

	/* #56 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555088
	/* java_name */
	.ascii	"android/content/pm/PackageInfo"
	.zero	72

	/* #57 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555090
	/* java_name */
	.ascii	"android/content/pm/PackageItemInfo"
	.zero	68

	/* #58 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555091
	/* java_name */
	.ascii	"android/content/pm/PackageManager"
	.zero	69

	/* #59 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555097
	/* java_name */
	.ascii	"android/content/res/AssetManager"
	.zero	70

	/* #60 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555098
	/* java_name */
	.ascii	"android/content/res/ColorStateList"
	.zero	68

	/* #61 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555099
	/* java_name */
	.ascii	"android/content/res/Configuration"
	.zero	69

	/* #62 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555102
	/* java_name */
	.ascii	"android/content/res/Resources"
	.zero	73

	/* #63 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555103
	/* java_name */
	.ascii	"android/content/res/Resources$Theme"
	.zero	67

	/* #64 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555104
	/* java_name */
	.ascii	"android/content/res/TypedArray"
	.zero	72

	/* #65 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555100
	/* java_name */
	.ascii	"android/content/res/XmlResourceParser"
	.zero	65

	/* #66 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554516
	/* java_name */
	.ascii	"android/database/DataSetObserver"
	.zero	70

	/* #67 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554926
	/* java_name */
	.ascii	"android/graphics/Bitmap"
	.zero	79

	/* #68 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554927
	/* java_name */
	.ascii	"android/graphics/Bitmap$CompressFormat"
	.zero	64

	/* #69 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554928
	/* java_name */
	.ascii	"android/graphics/Bitmap$Config"
	.zero	72

	/* #70 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554933
	/* java_name */
	.ascii	"android/graphics/BitmapFactory"
	.zero	72

	/* #71 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554934
	/* java_name */
	.ascii	"android/graphics/BitmapFactory$Options"
	.zero	64

	/* #72 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554930
	/* java_name */
	.ascii	"android/graphics/Canvas"
	.zero	79

	/* #73 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554940
	/* java_name */
	.ascii	"android/graphics/Color"
	.zero	80

	/* #74 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554939
	/* java_name */
	.ascii	"android/graphics/ColorFilter"
	.zero	74

	/* #75 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554942
	/* java_name */
	.ascii	"android/graphics/ImageFormat"
	.zero	74

	/* #76 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554944
	/* java_name */
	.ascii	"android/graphics/Matrix"
	.zero	79

	/* #77 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554945
	/* java_name */
	.ascii	"android/graphics/Paint"
	.zero	80

	/* #78 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554946
	/* java_name */
	.ascii	"android/graphics/Paint$Align"
	.zero	74

	/* #79 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554947
	/* java_name */
	.ascii	"android/graphics/Paint$FontMetricsInt"
	.zero	65

	/* #80 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554948
	/* java_name */
	.ascii	"android/graphics/Paint$Style"
	.zero	74

	/* #81 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554950
	/* java_name */
	.ascii	"android/graphics/Path"
	.zero	81

	/* #82 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554951
	/* java_name */
	.ascii	"android/graphics/Path$Direction"
	.zero	71

	/* #83 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554952
	/* java_name */
	.ascii	"android/graphics/Point"
	.zero	80

	/* #84 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554953
	/* java_name */
	.ascii	"android/graphics/PointF"
	.zero	79

	/* #85 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554954
	/* java_name */
	.ascii	"android/graphics/PorterDuff"
	.zero	75

	/* #86 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554955
	/* java_name */
	.ascii	"android/graphics/PorterDuff$Mode"
	.zero	70

	/* #87 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554956
	/* java_name */
	.ascii	"android/graphics/PorterDuffXfermode"
	.zero	67

	/* #88 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554957
	/* java_name */
	.ascii	"android/graphics/Rect"
	.zero	81

	/* #89 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554958
	/* java_name */
	.ascii	"android/graphics/RectF"
	.zero	80

	/* #90 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554959
	/* java_name */
	.ascii	"android/graphics/Typeface"
	.zero	77

	/* #91 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554961
	/* java_name */
	.ascii	"android/graphics/Xfermode"
	.zero	77

	/* #92 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554976
	/* java_name */
	.ascii	"android/graphics/drawable/Animatable"
	.zero	66

	/* #93 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554980
	/* java_name */
	.ascii	"android/graphics/drawable/Animatable2"
	.zero	65

	/* #94 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554977
	/* java_name */
	.ascii	"android/graphics/drawable/Animatable2$AnimationCallback"
	.zero	47

	/* #95 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554969
	/* java_name */
	.ascii	"android/graphics/drawable/AnimatedVectorDrawable"
	.zero	54

	/* #96 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554970
	/* java_name */
	.ascii	"android/graphics/drawable/AnimationDrawable"
	.zero	59

	/* #97 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554971
	/* java_name */
	.ascii	"android/graphics/drawable/BitmapDrawable"
	.zero	62

	/* #98 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554972
	/* java_name */
	.ascii	"android/graphics/drawable/ColorDrawable"
	.zero	63

	/* #99 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554962
	/* java_name */
	.ascii	"android/graphics/drawable/Drawable"
	.zero	68

	/* #100 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554964
	/* java_name */
	.ascii	"android/graphics/drawable/Drawable$Callback"
	.zero	59

	/* #101 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554965
	/* java_name */
	.ascii	"android/graphics/drawable/Drawable$ConstantState"
	.zero	54

	/* #102 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554967
	/* java_name */
	.ascii	"android/graphics/drawable/DrawableContainer"
	.zero	59

	/* #103 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554974
	/* java_name */
	.ascii	"android/graphics/drawable/GradientDrawable"
	.zero	60

	/* #104 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554968
	/* java_name */
	.ascii	"android/graphics/drawable/LayerDrawable"
	.zero	63

	/* #105 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554981
	/* java_name */
	.ascii	"android/graphics/drawable/RippleDrawable"
	.zero	62

	/* #106 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554983
	/* java_name */
	.ascii	"android/graphics/drawable/StateListDrawable"
	.zero	59

	/* #107 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554912
	/* java_name */
	.ascii	"android/hardware/Camera"
	.zero	79

	/* #108 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554913
	/* java_name */
	.ascii	"android/hardware/Camera$Area"
	.zero	74

	/* #109 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554915
	/* java_name */
	.ascii	"android/hardware/Camera$AutoFocusCallback"
	.zero	61

	/* #110 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554916
	/* java_name */
	.ascii	"android/hardware/Camera$CameraInfo"
	.zero	68

	/* #111 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554917
	/* java_name */
	.ascii	"android/hardware/Camera$Parameters"
	.zero	68

	/* #112 */
	/* module_index */
	.long	9
	/* type_token_id */
	.long	33554436
	/* java_name */
	.ascii	"android/hardware/Camera$PreviewCallback"
	.zero	63

	/* #113 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554918
	/* java_name */
	.ascii	"android/hardware/Camera$Size"
	.zero	74

	/* #114 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554920
	/* java_name */
	.ascii	"android/hardware/usb/UsbDevice"
	.zero	72

	/* #115 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554921
	/* java_name */
	.ascii	"android/hardware/usb/UsbDeviceConnection"
	.zero	62

	/* #116 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554922
	/* java_name */
	.ascii	"android/hardware/usb/UsbEndpoint"
	.zero	70

	/* #117 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554923
	/* java_name */
	.ascii	"android/hardware/usb/UsbInterface"
	.zero	69

	/* #118 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554924
	/* java_name */
	.ascii	"android/hardware/usb/UsbManager"
	.zero	71

	/* #119 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554901
	/* java_name */
	.ascii	"android/net/ConnectivityManager"
	.zero	71

	/* #120 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554904
	/* java_name */
	.ascii	"android/net/Network"
	.zero	83

	/* #121 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554905
	/* java_name */
	.ascii	"android/net/NetworkCapabilities"
	.zero	71

	/* #122 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554906
	/* java_name */
	.ascii	"android/net/NetworkInfo"
	.zero	79

	/* #123 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554907
	/* java_name */
	.ascii	"android/net/Uri"
	.zero	87

	/* #124 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554910
	/* java_name */
	.ascii	"android/net/wifi/WifiConfiguration"
	.zero	68

	/* #125 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554911
	/* java_name */
	.ascii	"android/net/wifi/WifiInfo"
	.zero	77

	/* #126 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554909
	/* java_name */
	.ascii	"android/net/wifi/WifiManager"
	.zero	74

	/* #127 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554871
	/* java_name */
	.ascii	"android/opengl/GLSurfaceView"
	.zero	74

	/* #128 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554873
	/* java_name */
	.ascii	"android/opengl/GLSurfaceView$Renderer"
	.zero	65

	/* #129 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554877
	/* java_name */
	.ascii	"android/os/AsyncTask"
	.zero	82

	/* #130 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554879
	/* java_name */
	.ascii	"android/os/BaseBundle"
	.zero	81

	/* #131 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554880
	/* java_name */
	.ascii	"android/os/Build"
	.zero	86

	/* #132 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554881
	/* java_name */
	.ascii	"android/os/Build$VERSION"
	.zero	78

	/* #133 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554883
	/* java_name */
	.ascii	"android/os/Bundle"
	.zero	85

	/* #134 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554884
	/* java_name */
	.ascii	"android/os/Environment"
	.zero	80

	/* #135 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554875
	/* java_name */
	.ascii	"android/os/Handler"
	.zero	84

	/* #136 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554888
	/* java_name */
	.ascii	"android/os/IBinder"
	.zero	84

	/* #137 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554886
	/* java_name */
	.ascii	"android/os/IBinder$DeathRecipient"
	.zero	69

	/* #138 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554890
	/* java_name */
	.ascii	"android/os/IInterface"
	.zero	81

	/* #139 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554895
	/* java_name */
	.ascii	"android/os/Looper"
	.zero	85

	/* #140 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554896
	/* java_name */
	.ascii	"android/os/Parcel"
	.zero	85

	/* #141 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554894
	/* java_name */
	.ascii	"android/os/Parcelable"
	.zero	81

	/* #142 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554892
	/* java_name */
	.ascii	"android/os/Parcelable$Creator"
	.zero	73

	/* #143 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554876
	/* java_name */
	.ascii	"android/os/PowerManager"
	.zero	79

	/* #144 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554898
	/* java_name */
	.ascii	"android/os/Process"
	.zero	84

	/* #145 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554899
	/* java_name */
	.ascii	"android/os/SystemClock"
	.zero	80

	/* #146 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554870
	/* java_name */
	.ascii	"android/preference/PreferenceManager"
	.zero	66

	/* #147 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554512
	/* java_name */
	.ascii	"android/provider/Settings"
	.zero	77

	/* #148 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554513
	/* java_name */
	.ascii	"android/provider/Settings$Global"
	.zero	70

	/* #149 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554514
	/* java_name */
	.ascii	"android/provider/Settings$NameValueTable"
	.zero	62

	/* #150 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554515
	/* java_name */
	.ascii	"android/provider/Settings$System"
	.zero	70

	/* #151 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555150
	/* java_name */
	.ascii	"android/runtime/JavaProxyThrowable"
	.zero	68

	/* #152 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555176
	/* java_name */
	.ascii	"android/runtime/XmlReaderPullParser"
	.zero	67

	/* #153 */
	/* module_index */
	.long	4
	/* type_token_id */
	.long	33554476
	/* java_name */
	.ascii	"android/support/design/internal/BottomNavigationItemView"
	.zero	46

	/* #154 */
	/* module_index */
	.long	4
	/* type_token_id */
	.long	33554477
	/* java_name */
	.ascii	"android/support/design/internal/BottomNavigationMenuView"
	.zero	46

	/* #155 */
	/* module_index */
	.long	4
	/* type_token_id */
	.long	33554478
	/* java_name */
	.ascii	"android/support/design/internal/BottomNavigationPresenter"
	.zero	45

	/* #156 */
	/* module_index */
	.long	4
	/* type_token_id */
	.long	33554450
	/* java_name */
	.ascii	"android/support/design/widget/AppBarLayout"
	.zero	60

	/* #157 */
	/* module_index */
	.long	4
	/* type_token_id */
	.long	33554451
	/* java_name */
	.ascii	"android/support/design/widget/AppBarLayout$LayoutParams"
	.zero	47

	/* #158 */
	/* module_index */
	.long	4
	/* type_token_id */
	.long	33554453
	/* java_name */
	.ascii	"android/support/design/widget/AppBarLayout$OnOffsetChangedListener"
	.zero	36

	/* #159 */
	/* module_index */
	.long	4
	/* type_token_id */
	.long	33554456
	/* java_name */
	.ascii	"android/support/design/widget/AppBarLayout$ScrollingViewBehavior"
	.zero	38

	/* #160 */
	/* module_index */
	.long	4
	/* type_token_id */
	.long	33554459
	/* java_name */
	.ascii	"android/support/design/widget/BottomNavigationView"
	.zero	52

	/* #161 */
	/* module_index */
	.long	4
	/* type_token_id */
	.long	33554461
	/* java_name */
	.ascii	"android/support/design/widget/BottomNavigationView$OnNavigationItemReselectedListener"
	.zero	17

	/* #162 */
	/* module_index */
	.long	4
	/* type_token_id */
	.long	33554465
	/* java_name */
	.ascii	"android/support/design/widget/BottomNavigationView$OnNavigationItemSelectedListener"
	.zero	19

	/* #163 */
	/* module_index */
	.long	4
	/* type_token_id */
	.long	33554472
	/* java_name */
	.ascii	"android/support/design/widget/BottomSheetDialog"
	.zero	55

	/* #164 */
	/* module_index */
	.long	10
	/* type_token_id */
	.long	33554435
	/* java_name */
	.ascii	"android/support/design/widget/CoordinatorLayout"
	.zero	55

	/* #165 */
	/* module_index */
	.long	10
	/* type_token_id */
	.long	33554436
	/* java_name */
	.ascii	"android/support/design/widget/CoordinatorLayout$Behavior"
	.zero	46

	/* #166 */
	/* module_index */
	.long	10
	/* type_token_id */
	.long	33554438
	/* java_name */
	.ascii	"android/support/design/widget/CoordinatorLayout$LayoutParams"
	.zero	42

	/* #167 */
	/* module_index */
	.long	4
	/* type_token_id */
	.long	33554473
	/* java_name */
	.ascii	"android/support/design/widget/HeaderScrollingViewBehavior"
	.zero	45

	/* #168 */
	/* module_index */
	.long	4
	/* type_token_id */
	.long	33554435
	/* java_name */
	.ascii	"android/support/design/widget/TabLayout"
	.zero	63

	/* #169 */
	/* module_index */
	.long	4
	/* type_token_id */
	.long	33554438
	/* java_name */
	.ascii	"android/support/design/widget/TabLayout$BaseOnTabSelectedListener"
	.zero	37

	/* #170 */
	/* module_index */
	.long	4
	/* type_token_id */
	.long	33554443
	/* java_name */
	.ascii	"android/support/design/widget/TabLayout$Tab"
	.zero	59

	/* #171 */
	/* module_index */
	.long	4
	/* type_token_id */
	.long	33554436
	/* java_name */
	.ascii	"android/support/design/widget/TabLayout$TabView"
	.zero	55

	/* #172 */
	/* module_index */
	.long	4
	/* type_token_id */
	.long	33554475
	/* java_name */
	.ascii	"android/support/design/widget/ViewOffsetBehavior"
	.zero	54

	/* #173 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554435
	/* java_name */
	.ascii	"android/support/v13/view/DragAndDropPermissionsCompat"
	.zero	49

	/* #174 */
	/* module_index */
	.long	24
	/* type_token_id */
	.long	33554435
	/* java_name */
	.ascii	"android/support/v4/app/ActionBarDrawerToggle"
	.zero	58

	/* #175 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554508
	/* java_name */
	.ascii	"android/support/v4/app/ActivityCompat"
	.zero	65

	/* #176 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554510
	/* java_name */
	.ascii	"android/support/v4/app/ActivityCompat$OnRequestPermissionsResultCallback"
	.zero	30

	/* #177 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554512
	/* java_name */
	.ascii	"android/support/v4/app/ActivityCompat$PermissionCompatDelegate"
	.zero	40

	/* #178 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554514
	/* java_name */
	.ascii	"android/support/v4/app/ActivityCompat$RequestPermissionsRequestCodeValidator"
	.zero	26

	/* #179 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554436
	/* java_name */
	.ascii	"android/support/v4/app/Fragment"
	.zero	71

	/* #180 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554437
	/* java_name */
	.ascii	"android/support/v4/app/Fragment$SavedState"
	.zero	60

	/* #181 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554435
	/* java_name */
	.ascii	"android/support/v4/app/FragmentActivity"
	.zero	63

	/* #182 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554438
	/* java_name */
	.ascii	"android/support/v4/app/FragmentManager"
	.zero	64

	/* #183 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554440
	/* java_name */
	.ascii	"android/support/v4/app/FragmentManager$BackStackEntry"
	.zero	49

	/* #184 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554441
	/* java_name */
	.ascii	"android/support/v4/app/FragmentManager$FragmentLifecycleCallbacks"
	.zero	37

	/* #185 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554444
	/* java_name */
	.ascii	"android/support/v4/app/FragmentManager$OnBackStackChangedListener"
	.zero	37

	/* #186 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554449
	/* java_name */
	.ascii	"android/support/v4/app/FragmentPagerAdapter"
	.zero	59

	/* #187 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554451
	/* java_name */
	.ascii	"android/support/v4/app/FragmentTransaction"
	.zero	60

	/* #188 */
	/* module_index */
	.long	0
	/* type_token_id */
	.long	33554440
	/* java_name */
	.ascii	"android/support/v4/app/LoaderManager"
	.zero	66

	/* #189 */
	/* module_index */
	.long	0
	/* type_token_id */
	.long	33554442
	/* java_name */
	.ascii	"android/support/v4/app/LoaderManager$LoaderCallbacks"
	.zero	50

	/* #190 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554515
	/* java_name */
	.ascii	"android/support/v4/app/SharedElementCallback"
	.zero	58

	/* #191 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554517
	/* java_name */
	.ascii	"android/support/v4/app/SharedElementCallback$OnSharedElementsReadyListener"
	.zero	28

	/* #192 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554519
	/* java_name */
	.ascii	"android/support/v4/app/TaskStackBuilder"
	.zero	63

	/* #193 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554521
	/* java_name */
	.ascii	"android/support/v4/app/TaskStackBuilder$SupportParentable"
	.zero	45

	/* #194 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554507
	/* java_name */
	.ascii	"android/support/v4/content/ContextCompat"
	.zero	62

	/* #195 */
	/* module_index */
	.long	0
	/* type_token_id */
	.long	33554435
	/* java_name */
	.ascii	"android/support/v4/content/Loader"
	.zero	69

	/* #196 */
	/* module_index */
	.long	0
	/* type_token_id */
	.long	33554437
	/* java_name */
	.ascii	"android/support/v4/content/Loader$OnLoadCanceledListener"
	.zero	46

	/* #197 */
	/* module_index */
	.long	0
	/* type_token_id */
	.long	33554439
	/* java_name */
	.ascii	"android/support/v4/content/Loader$OnLoadCompleteListener"
	.zero	46

	/* #198 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554506
	/* java_name */
	.ascii	"android/support/v4/graphics/drawable/DrawableCompat"
	.zero	51

	/* #199 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554503
	/* java_name */
	.ascii	"android/support/v4/internal/view/SupportMenu"
	.zero	58

	/* #200 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554505
	/* java_name */
	.ascii	"android/support/v4/internal/view/SupportMenuItem"
	.zero	54

	/* #201 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554522
	/* java_name */
	.ascii	"android/support/v4/text/PrecomputedTextCompat"
	.zero	57

	/* #202 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554523
	/* java_name */
	.ascii	"android/support/v4/text/PrecomputedTextCompat$Params"
	.zero	50

	/* #203 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554451
	/* java_name */
	.ascii	"android/support/v4/view/AccessibilityDelegateCompat"
	.zero	51

	/* #204 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554452
	/* java_name */
	.ascii	"android/support/v4/view/ActionProvider"
	.zero	64

	/* #205 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554454
	/* java_name */
	.ascii	"android/support/v4/view/ActionProvider$SubUiVisibilityListener"
	.zero	40

	/* #206 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554458
	/* java_name */
	.ascii	"android/support/v4/view/ActionProvider$VisibilityListener"
	.zero	45

	/* #207 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554466
	/* java_name */
	.ascii	"android/support/v4/view/DisplayCutoutCompat"
	.zero	59

	/* #208 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554485
	/* java_name */
	.ascii	"android/support/v4/view/MenuItemCompat"
	.zero	64

	/* #209 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554487
	/* java_name */
	.ascii	"android/support/v4/view/MenuItemCompat$OnActionExpandListener"
	.zero	41

	/* #210 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554468
	/* java_name */
	.ascii	"android/support/v4/view/NestedScrollingChild"
	.zero	58

	/* #211 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554470
	/* java_name */
	.ascii	"android/support/v4/view/NestedScrollingChild2"
	.zero	57

	/* #212 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554472
	/* java_name */
	.ascii	"android/support/v4/view/NestedScrollingParent"
	.zero	57

	/* #213 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554474
	/* java_name */
	.ascii	"android/support/v4/view/NestedScrollingParent2"
	.zero	56

	/* #214 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554476
	/* java_name */
	.ascii	"android/support/v4/view/OnApplyWindowInsetsListener"
	.zero	51

	/* #215 */
	/* module_index */
	.long	23
	/* type_token_id */
	.long	33554435
	/* java_name */
	.ascii	"android/support/v4/view/PagerAdapter"
	.zero	66

	/* #216 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554488
	/* java_name */
	.ascii	"android/support/v4/view/PointerIconCompat"
	.zero	61

	/* #217 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554489
	/* java_name */
	.ascii	"android/support/v4/view/ScaleGestureDetectorCompat"
	.zero	52

	/* #218 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554478
	/* java_name */
	.ascii	"android/support/v4/view/ScrollingView"
	.zero	65

	/* #219 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554480
	/* java_name */
	.ascii	"android/support/v4/view/TintableBackgroundView"
	.zero	56

	/* #220 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554490
	/* java_name */
	.ascii	"android/support/v4/view/ViewCompat"
	.zero	68

	/* #221 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554492
	/* java_name */
	.ascii	"android/support/v4/view/ViewCompat$OnUnhandledKeyEventListenerCompat"
	.zero	34

	/* #222 */
	/* module_index */
	.long	23
	/* type_token_id */
	.long	33554437
	/* java_name */
	.ascii	"android/support/v4/view/ViewPager"
	.zero	69

	/* #223 */
	/* module_index */
	.long	23
	/* type_token_id */
	.long	33554439
	/* java_name */
	.ascii	"android/support/v4/view/ViewPager$OnAdapterChangeListener"
	.zero	45

	/* #224 */
	/* module_index */
	.long	23
	/* type_token_id */
	.long	33554443
	/* java_name */
	.ascii	"android/support/v4/view/ViewPager$OnPageChangeListener"
	.zero	48

	/* #225 */
	/* module_index */
	.long	23
	/* type_token_id */
	.long	33554449
	/* java_name */
	.ascii	"android/support/v4/view/ViewPager$PageTransformer"
	.zero	53

	/* #226 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554493
	/* java_name */
	.ascii	"android/support/v4/view/ViewPropertyAnimatorCompat"
	.zero	52

	/* #227 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554482
	/* java_name */
	.ascii	"android/support/v4/view/ViewPropertyAnimatorListener"
	.zero	50

	/* #228 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554484
	/* java_name */
	.ascii	"android/support/v4/view/ViewPropertyAnimatorUpdateListener"
	.zero	44

	/* #229 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554494
	/* java_name */
	.ascii	"android/support/v4/view/WindowInsetsCompat"
	.zero	60

	/* #230 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554495
	/* java_name */
	.ascii	"android/support/v4/view/accessibility/AccessibilityNodeInfoCompat"
	.zero	37

	/* #231 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554496
	/* java_name */
	.ascii	"android/support/v4/view/accessibility/AccessibilityNodeInfoCompat$AccessibilityActionCompat"
	.zero	11

	/* #232 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554497
	/* java_name */
	.ascii	"android/support/v4/view/accessibility/AccessibilityNodeInfoCompat$CollectionInfoCompat"
	.zero	16

	/* #233 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554498
	/* java_name */
	.ascii	"android/support/v4/view/accessibility/AccessibilityNodeInfoCompat$CollectionItemInfoCompat"
	.zero	12

	/* #234 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554499
	/* java_name */
	.ascii	"android/support/v4/view/accessibility/AccessibilityNodeInfoCompat$RangeInfoCompat"
	.zero	21

	/* #235 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554500
	/* java_name */
	.ascii	"android/support/v4/view/accessibility/AccessibilityNodeProviderCompat"
	.zero	33

	/* #236 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554501
	/* java_name */
	.ascii	"android/support/v4/view/accessibility/AccessibilityWindowInfoCompat"
	.zero	35

	/* #237 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554438
	/* java_name */
	.ascii	"android/support/v4/widget/AutoSizeableTextView"
	.zero	56

	/* #238 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554436
	/* java_name */
	.ascii	"android/support/v4/widget/CompoundButtonCompat"
	.zero	56

	/* #239 */
	/* module_index */
	.long	11
	/* type_token_id */
	.long	33554435
	/* java_name */
	.ascii	"android/support/v4/widget/DrawerLayout"
	.zero	64

	/* #240 */
	/* module_index */
	.long	11
	/* type_token_id */
	.long	33554437
	/* java_name */
	.ascii	"android/support/v4/widget/DrawerLayout$DrawerListener"
	.zero	49

	/* #241 */
	/* module_index */
	.long	11
	/* type_token_id */
	.long	33554443
	/* java_name */
	.ascii	"android/support/v4/widget/DrawerLayout$LayoutParams"
	.zero	51

	/* #242 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554443
	/* java_name */
	.ascii	"android/support/v4/widget/NestedScrollView"
	.zero	60

	/* #243 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554445
	/* java_name */
	.ascii	"android/support/v4/widget/NestedScrollView$OnScrollChangeListener"
	.zero	37

	/* #244 */
	/* module_index */
	.long	25
	/* type_token_id */
	.long	33554435
	/* java_name */
	.ascii	"android/support/v4/widget/SwipeRefreshLayout"
	.zero	58

	/* #245 */
	/* module_index */
	.long	25
	/* type_token_id */
	.long	33554437
	/* java_name */
	.ascii	"android/support/v4/widget/SwipeRefreshLayout$OnChildScrollUpCallback"
	.zero	34

	/* #246 */
	/* module_index */
	.long	25
	/* type_token_id */
	.long	33554439
	/* java_name */
	.ascii	"android/support/v4/widget/SwipeRefreshLayout$OnRefreshListener"
	.zero	40

	/* #247 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554450
	/* java_name */
	.ascii	"android/support/v4/widget/TextViewCompat"
	.zero	62

	/* #248 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554440
	/* java_name */
	.ascii	"android/support/v4/widget/TintableCompoundButton"
	.zero	54

	/* #249 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554442
	/* java_name */
	.ascii	"android/support/v4/widget/TintableImageSourceView"
	.zero	53

	/* #250 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554438
	/* java_name */
	.ascii	"android/support/v7/app/ActionBar"
	.zero	70

	/* #251 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554439
	/* java_name */
	.ascii	"android/support/v7/app/ActionBar$LayoutParams"
	.zero	57

	/* #252 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554441
	/* java_name */
	.ascii	"android/support/v7/app/ActionBar$OnMenuVisibilityListener"
	.zero	45

	/* #253 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554445
	/* java_name */
	.ascii	"android/support/v7/app/ActionBar$OnNavigationListener"
	.zero	49

	/* #254 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554446
	/* java_name */
	.ascii	"android/support/v7/app/ActionBar$Tab"
	.zero	66

	/* #255 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554449
	/* java_name */
	.ascii	"android/support/v7/app/ActionBar$TabListener"
	.zero	58

	/* #256 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554453
	/* java_name */
	.ascii	"android/support/v7/app/ActionBarDrawerToggle"
	.zero	58

	/* #257 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554455
	/* java_name */
	.ascii	"android/support/v7/app/ActionBarDrawerToggle$Delegate"
	.zero	49

	/* #258 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554457
	/* java_name */
	.ascii	"android/support/v7/app/ActionBarDrawerToggle$DelegateProvider"
	.zero	41

	/* #259 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554458
	/* java_name */
	.ascii	"android/support/v7/app/AppCompatActivity"
	.zero	62

	/* #260 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554463
	/* java_name */
	.ascii	"android/support/v7/app/AppCompatCallback"
	.zero	62

	/* #261 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554459
	/* java_name */
	.ascii	"android/support/v7/app/AppCompatDelegate"
	.zero	62

	/* #262 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554461
	/* java_name */
	.ascii	"android/support/v7/app/AppCompatDialog"
	.zero	64

	/* #263 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554437
	/* java_name */
	.ascii	"android/support/v7/content/res/AppCompatResources"
	.zero	53

	/* #264 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554435
	/* java_name */
	.ascii	"android/support/v7/graphics/drawable/DrawableWrapper"
	.zero	50

	/* #265 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554436
	/* java_name */
	.ascii	"android/support/v7/graphics/drawable/DrawerArrowDrawable"
	.zero	46

	/* #266 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554483
	/* java_name */
	.ascii	"android/support/v7/view/ActionMode"
	.zero	68

	/* #267 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554485
	/* java_name */
	.ascii	"android/support/v7/view/ActionMode$Callback"
	.zero	59

	/* #268 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554487
	/* java_name */
	.ascii	"android/support/v7/view/menu/MenuBuilder"
	.zero	62

	/* #269 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554489
	/* java_name */
	.ascii	"android/support/v7/view/menu/MenuBuilder$Callback"
	.zero	53

	/* #270 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554498
	/* java_name */
	.ascii	"android/support/v7/view/menu/MenuItemImpl"
	.zero	61

	/* #271 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554493
	/* java_name */
	.ascii	"android/support/v7/view/menu/MenuPresenter"
	.zero	60

	/* #272 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554491
	/* java_name */
	.ascii	"android/support/v7/view/menu/MenuPresenter$Callback"
	.zero	51

	/* #273 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554497
	/* java_name */
	.ascii	"android/support/v7/view/menu/MenuView"
	.zero	65

	/* #274 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554495
	/* java_name */
	.ascii	"android/support/v7/view/menu/MenuView$ItemView"
	.zero	56

	/* #275 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554499
	/* java_name */
	.ascii	"android/support/v7/view/menu/SubMenuBuilder"
	.zero	59

	/* #276 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554474
	/* java_name */
	.ascii	"android/support/v7/widget/AppCompatAutoCompleteTextView"
	.zero	47

	/* #277 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554475
	/* java_name */
	.ascii	"android/support/v7/widget/AppCompatButton"
	.zero	61

	/* #278 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554476
	/* java_name */
	.ascii	"android/support/v7/widget/AppCompatCheckBox"
	.zero	59

	/* #279 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554477
	/* java_name */
	.ascii	"android/support/v7/widget/AppCompatImageButton"
	.zero	56

	/* #280 */
	/* module_index */
	.long	14
	/* type_token_id */
	.long	33554435
	/* java_name */
	.ascii	"android/support/v7/widget/CardView"
	.zero	68

	/* #281 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554479
	/* java_name */
	.ascii	"android/support/v7/widget/DecorToolbar"
	.zero	64

	/* #282 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554435
	/* java_name */
	.ascii	"android/support/v7/widget/GridLayoutManager"
	.zero	59

	/* #283 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554436
	/* java_name */
	.ascii	"android/support/v7/widget/GridLayoutManager$LayoutParams"
	.zero	46

	/* #284 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554437
	/* java_name */
	.ascii	"android/support/v7/widget/GridLayoutManager$SpanSizeLookup"
	.zero	44

	/* #285 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554439
	/* java_name */
	.ascii	"android/support/v7/widget/LinearLayoutManager"
	.zero	57

	/* #286 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554440
	/* java_name */
	.ascii	"android/support/v7/widget/LinearSmoothScroller"
	.zero	56

	/* #287 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554441
	/* java_name */
	.ascii	"android/support/v7/widget/LinearSnapHelper"
	.zero	60

	/* #288 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554442
	/* java_name */
	.ascii	"android/support/v7/widget/OrientationHelper"
	.zero	59

	/* #289 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554444
	/* java_name */
	.ascii	"android/support/v7/widget/PagerSnapHelper"
	.zero	61

	/* #290 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554445
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView"
	.zero	64

	/* #291 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554446
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$Adapter"
	.zero	56

	/* #292 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554448
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$AdapterDataObserver"
	.zero	44

	/* #293 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554451
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$ChildDrawingOrderCallback"
	.zero	38

	/* #294 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554452
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$EdgeEffectFactory"
	.zero	46

	/* #295 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554453
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$ItemAnimator"
	.zero	51

	/* #296 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554455
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$ItemAnimator$ItemAnimatorFinishedListener"
	.zero	22

	/* #297 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554456
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$ItemAnimator$ItemHolderInfo"
	.zero	36

	/* #298 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554458
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$ItemDecoration"
	.zero	49

	/* #299 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554460
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$LayoutManager"
	.zero	50

	/* #300 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554462
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$LayoutManager$LayoutPrefetchRegistry"
	.zero	27

	/* #301 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554463
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$LayoutManager$Properties"
	.zero	39

	/* #302 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554465
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$LayoutParams"
	.zero	51

	/* #303 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554467
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$OnChildAttachStateChangeListener"
	.zero	31

	/* #304 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554471
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$OnFlingListener"
	.zero	48

	/* #305 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554474
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$OnItemTouchListener"
	.zero	44

	/* #306 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554479
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$OnScrollListener"
	.zero	47

	/* #307 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554481
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$RecycledViewPool"
	.zero	47

	/* #308 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554482
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$Recycler"
	.zero	55

	/* #309 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554484
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$RecyclerListener"
	.zero	47

	/* #310 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554487
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$SmoothScroller"
	.zero	49

	/* #311 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554488
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$SmoothScroller$Action"
	.zero	42

	/* #312 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554490
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$SmoothScroller$ScrollVectorProvider"
	.zero	28

	/* #313 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554492
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$State"
	.zero	58

	/* #314 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554493
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$ViewCacheExtension"
	.zero	45

	/* #315 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554495
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerView$ViewHolder"
	.zero	53

	/* #316 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554509
	/* java_name */
	.ascii	"android/support/v7/widget/RecyclerViewAccessibilityDelegate"
	.zero	43

	/* #317 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554480
	/* java_name */
	.ascii	"android/support/v7/widget/ScrollingTabContainerView"
	.zero	51

	/* #318 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554481
	/* java_name */
	.ascii	"android/support/v7/widget/ScrollingTabContainerView$VisibilityAnimListener"
	.zero	28

	/* #319 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554510
	/* java_name */
	.ascii	"android/support/v7/widget/SnapHelper"
	.zero	66

	/* #320 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554482
	/* java_name */
	.ascii	"android/support/v7/widget/SwitchCompat"
	.zero	64

	/* #321 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554464
	/* java_name */
	.ascii	"android/support/v7/widget/Toolbar"
	.zero	69

	/* #322 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554467
	/* java_name */
	.ascii	"android/support/v7/widget/Toolbar$LayoutParams"
	.zero	56

	/* #323 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554469
	/* java_name */
	.ascii	"android/support/v7/widget/Toolbar$OnMenuItemClickListener"
	.zero	45

	/* #324 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554465
	/* java_name */
	.ascii	"android/support/v7/widget/Toolbar_NavigationOnClickEventDispatcher"
	.zero	36

	/* #325 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554514
	/* java_name */
	.ascii	"android/support/v7/widget/helper/ItemTouchHelper"
	.zero	54

	/* #326 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554515
	/* java_name */
	.ascii	"android/support/v7/widget/helper/ItemTouchHelper$Callback"
	.zero	45

	/* #327 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554518
	/* java_name */
	.ascii	"android/support/v7/widget/helper/ItemTouchHelper$ViewDropHandler"
	.zero	38

	/* #328 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554513
	/* java_name */
	.ascii	"android/support/v7/widget/helper/ItemTouchUIUtil"
	.zero	54

	/* #329 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554800
	/* java_name */
	.ascii	"android/text/ClipboardManager"
	.zero	73

	/* #330 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554805
	/* java_name */
	.ascii	"android/text/Editable"
	.zero	81

	/* #331 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554808
	/* java_name */
	.ascii	"android/text/GetChars"
	.zero	81

	/* #332 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554803
	/* java_name */
	.ascii	"android/text/Html"
	.zero	85

	/* #333 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554812
	/* java_name */
	.ascii	"android/text/InputFilter"
	.zero	78

	/* #334 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554810
	/* java_name */
	.ascii	"android/text/InputFilter$LengthFilter"
	.zero	65

	/* #335 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554828
	/* java_name */
	.ascii	"android/text/Layout"
	.zero	83

	/* #336 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554829
	/* java_name */
	.ascii	"android/text/Layout$Alignment"
	.zero	73

	/* #337 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554814
	/* java_name */
	.ascii	"android/text/NoCopySpan"
	.zero	79

	/* #338 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554817
	/* java_name */
	.ascii	"android/text/ParcelableSpan"
	.zero	75

	/* #339 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554819
	/* java_name */
	.ascii	"android/text/Spannable"
	.zero	80

	/* #340 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554831
	/* java_name */
	.ascii	"android/text/SpannableString"
	.zero	74

	/* #341 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554833
	/* java_name */
	.ascii	"android/text/SpannableStringBuilder"
	.zero	67

	/* #342 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554835
	/* java_name */
	.ascii	"android/text/SpannableStringInternal"
	.zero	66

	/* #343 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554822
	/* java_name */
	.ascii	"android/text/Spanned"
	.zero	82

	/* #344 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554838
	/* java_name */
	.ascii	"android/text/StaticLayout"
	.zero	77

	/* #345 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554825
	/* java_name */
	.ascii	"android/text/TextDirectionHeuristic"
	.zero	67

	/* #346 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554839
	/* java_name */
	.ascii	"android/text/TextPaint"
	.zero	80

	/* #347 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554840
	/* java_name */
	.ascii	"android/text/TextUtils"
	.zero	80

	/* #348 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554841
	/* java_name */
	.ascii	"android/text/TextUtils$TruncateAt"
	.zero	69

	/* #349 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554827
	/* java_name */
	.ascii	"android/text/TextWatcher"
	.zero	78

	/* #350 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554869
	/* java_name */
	.ascii	"android/text/format/DateFormat"
	.zero	72

	/* #351 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554858
	/* java_name */
	.ascii	"android/text/method/BaseKeyListener"
	.zero	67

	/* #352 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554860
	/* java_name */
	.ascii	"android/text/method/DigitsKeyListener"
	.zero	65

	/* #353 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554862
	/* java_name */
	.ascii	"android/text/method/KeyListener"
	.zero	71

	/* #354 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554865
	/* java_name */
	.ascii	"android/text/method/MetaKeyKeyListener"
	.zero	64

	/* #355 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554867
	/* java_name */
	.ascii	"android/text/method/NumberKeyListener"
	.zero	65

	/* #356 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554864
	/* java_name */
	.ascii	"android/text/method/TransformationMethod"
	.zero	62

	/* #357 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554842
	/* java_name */
	.ascii	"android/text/style/BackgroundColorSpan"
	.zero	64

	/* #358 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554843
	/* java_name */
	.ascii	"android/text/style/CharacterStyle"
	.zero	69

	/* #359 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554845
	/* java_name */
	.ascii	"android/text/style/ForegroundColorSpan"
	.zero	64

	/* #360 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554847
	/* java_name */
	.ascii	"android/text/style/LineHeightSpan"
	.zero	69

	/* #361 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554856
	/* java_name */
	.ascii	"android/text/style/MetricAffectingSpan"
	.zero	64

	/* #362 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554849
	/* java_name */
	.ascii	"android/text/style/ParagraphStyle"
	.zero	69

	/* #363 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554851
	/* java_name */
	.ascii	"android/text/style/UpdateAppearance"
	.zero	67

	/* #364 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554853
	/* java_name */
	.ascii	"android/text/style/UpdateLayout"
	.zero	71

	/* #365 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554855
	/* java_name */
	.ascii	"android/text/style/WrapTogetherSpan"
	.zero	67

	/* #366 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554795
	/* java_name */
	.ascii	"android/util/AttributeSet"
	.zero	77

	/* #367 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554792
	/* java_name */
	.ascii	"android/util/DisplayMetrics"
	.zero	75

	/* #368 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554790
	/* java_name */
	.ascii	"android/util/Log"
	.zero	86

	/* #369 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554796
	/* java_name */
	.ascii	"android/util/LruCache"
	.zero	81

	/* #370 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554797
	/* java_name */
	.ascii	"android/util/SparseArray"
	.zero	78

	/* #371 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554798
	/* java_name */
	.ascii	"android/util/StateSet"
	.zero	81

	/* #372 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554799
	/* java_name */
	.ascii	"android/util/TypedValue"
	.zero	79

	/* #373 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554671
	/* java_name */
	.ascii	"android/view/ActionMode"
	.zero	79

	/* #374 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554673
	/* java_name */
	.ascii	"android/view/ActionMode$Callback"
	.zero	70

	/* #375 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554676
	/* java_name */
	.ascii	"android/view/ActionProvider"
	.zero	75

	/* #376 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554692
	/* java_name */
	.ascii	"android/view/CollapsibleActionView"
	.zero	68

	/* #377 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554696
	/* java_name */
	.ascii	"android/view/ContextMenu"
	.zero	78

	/* #378 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554694
	/* java_name */
	.ascii	"android/view/ContextMenu$ContextMenuInfo"
	.zero	62

	/* #379 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554679
	/* java_name */
	.ascii	"android/view/ContextThemeWrapper"
	.zero	70

	/* #380 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554681
	/* java_name */
	.ascii	"android/view/Display"
	.zero	82

	/* #381 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554682
	/* java_name */
	.ascii	"android/view/DragEvent"
	.zero	80

	/* #382 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554685
	/* java_name */
	.ascii	"android/view/GestureDetector"
	.zero	74

	/* #383 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554687
	/* java_name */
	.ascii	"android/view/GestureDetector$OnDoubleTapListener"
	.zero	54

	/* #384 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554689
	/* java_name */
	.ascii	"android/view/GestureDetector$OnGestureListener"
	.zero	56

	/* #385 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554708
	/* java_name */
	.ascii	"android/view/InputEvent"
	.zero	79

	/* #386 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554650
	/* java_name */
	.ascii	"android/view/KeyEvent"
	.zero	81

	/* #387 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554652
	/* java_name */
	.ascii	"android/view/KeyEvent$Callback"
	.zero	72

	/* #388 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554653
	/* java_name */
	.ascii	"android/view/LayoutInflater"
	.zero	75

	/* #389 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554655
	/* java_name */
	.ascii	"android/view/LayoutInflater$Factory"
	.zero	67

	/* #390 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554657
	/* java_name */
	.ascii	"android/view/LayoutInflater$Factory2"
	.zero	66

	/* #391 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554699
	/* java_name */
	.ascii	"android/view/Menu"
	.zero	85

	/* #392 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554732
	/* java_name */
	.ascii	"android/view/MenuInflater"
	.zero	77

	/* #393 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554706
	/* java_name */
	.ascii	"android/view/MenuItem"
	.zero	81

	/* #394 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554701
	/* java_name */
	.ascii	"android/view/MenuItem$OnActionExpandListener"
	.zero	58

	/* #395 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554703
	/* java_name */
	.ascii	"android/view/MenuItem$OnMenuItemClickListener"
	.zero	57

	/* #396 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554658
	/* java_name */
	.ascii	"android/view/MotionEvent"
	.zero	78

	/* #397 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554737
	/* java_name */
	.ascii	"android/view/ScaleGestureDetector"
	.zero	69

	/* #398 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554739
	/* java_name */
	.ascii	"android/view/ScaleGestureDetector$OnScaleGestureListener"
	.zero	46

	/* #399 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554740
	/* java_name */
	.ascii	"android/view/ScaleGestureDetector$SimpleOnScaleGestureListener"
	.zero	40

	/* #400 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554742
	/* java_name */
	.ascii	"android/view/SearchEvent"
	.zero	78

	/* #401 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554711
	/* java_name */
	.ascii	"android/view/SubMenu"
	.zero	82

	/* #402 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554745
	/* java_name */
	.ascii	"android/view/Surface"
	.zero	82

	/* #403 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554717
	/* java_name */
	.ascii	"android/view/SurfaceHolder"
	.zero	76

	/* #404 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554713
	/* java_name */
	.ascii	"android/view/SurfaceHolder$Callback"
	.zero	67

	/* #405 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554715
	/* java_name */
	.ascii	"android/view/SurfaceHolder$Callback2"
	.zero	66

	/* #406 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554748
	/* java_name */
	.ascii	"android/view/SurfaceView"
	.zero	78

	/* #407 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554606
	/* java_name */
	.ascii	"android/view/View"
	.zero	85

	/* #408 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554607
	/* java_name */
	.ascii	"android/view/View$AccessibilityDelegate"
	.zero	63

	/* #409 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554608
	/* java_name */
	.ascii	"android/view/View$DragShadowBuilder"
	.zero	67

	/* #410 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554609
	/* java_name */
	.ascii	"android/view/View$MeasureSpec"
	.zero	73

	/* #411 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554611
	/* java_name */
	.ascii	"android/view/View$OnAttachStateChangeListener"
	.zero	57

	/* #412 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554616
	/* java_name */
	.ascii	"android/view/View$OnClickListener"
	.zero	69

	/* #413 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554619
	/* java_name */
	.ascii	"android/view/View$OnCreateContextMenuListener"
	.zero	57

	/* #414 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554621
	/* java_name */
	.ascii	"android/view/View$OnFocusChangeListener"
	.zero	63

	/* #415 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554623
	/* java_name */
	.ascii	"android/view/View$OnKeyListener"
	.zero	71

	/* #416 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554627
	/* java_name */
	.ascii	"android/view/View$OnLayoutChangeListener"
	.zero	62

	/* #417 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554631
	/* java_name */
	.ascii	"android/view/View$OnScrollChangeListener"
	.zero	62

	/* #418 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554635
	/* java_name */
	.ascii	"android/view/View$OnTouchListener"
	.zero	69

	/* #419 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554751
	/* java_name */
	.ascii	"android/view/ViewConfiguration"
	.zero	72

	/* #420 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554752
	/* java_name */
	.ascii	"android/view/ViewGroup"
	.zero	80

	/* #421 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554753
	/* java_name */
	.ascii	"android/view/ViewGroup$LayoutParams"
	.zero	67

	/* #422 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554754
	/* java_name */
	.ascii	"android/view/ViewGroup$MarginLayoutParams"
	.zero	61

	/* #423 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554756
	/* java_name */
	.ascii	"android/view/ViewGroup$OnHierarchyChangeListener"
	.zero	54

	/* #424 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554719
	/* java_name */
	.ascii	"android/view/ViewManager"
	.zero	78

	/* #425 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554721
	/* java_name */
	.ascii	"android/view/ViewParent"
	.zero	79

	/* #426 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554758
	/* java_name */
	.ascii	"android/view/ViewPropertyAnimator"
	.zero	69

	/* #427 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554659
	/* java_name */
	.ascii	"android/view/ViewTreeObserver"
	.zero	73

	/* #428 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554661
	/* java_name */
	.ascii	"android/view/ViewTreeObserver$OnGlobalFocusChangeListener"
	.zero	45

	/* #429 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554663
	/* java_name */
	.ascii	"android/view/ViewTreeObserver$OnGlobalLayoutListener"
	.zero	50

	/* #430 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554665
	/* java_name */
	.ascii	"android/view/ViewTreeObserver$OnPreDrawListener"
	.zero	55

	/* #431 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554667
	/* java_name */
	.ascii	"android/view/ViewTreeObserver$OnTouchModeChangeListener"
	.zero	47

	/* #432 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554668
	/* java_name */
	.ascii	"android/view/Window"
	.zero	83

	/* #433 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554670
	/* java_name */
	.ascii	"android/view/Window$Callback"
	.zero	74

	/* #434 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554762
	/* java_name */
	.ascii	"android/view/WindowInsets"
	.zero	77

	/* #435 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554724
	/* java_name */
	.ascii	"android/view/WindowManager"
	.zero	76

	/* #436 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554722
	/* java_name */
	.ascii	"android/view/WindowManager$LayoutParams"
	.zero	63

	/* #437 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554781
	/* java_name */
	.ascii	"android/view/accessibility/AccessibilityEvent"
	.zero	57

	/* #438 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554789
	/* java_name */
	.ascii	"android/view/accessibility/AccessibilityEventSource"
	.zero	51

	/* #439 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554782
	/* java_name */
	.ascii	"android/view/accessibility/AccessibilityManager"
	.zero	55

	/* #440 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554783
	/* java_name */
	.ascii	"android/view/accessibility/AccessibilityNodeInfo"
	.zero	54

	/* #441 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554784
	/* java_name */
	.ascii	"android/view/accessibility/AccessibilityRecord"
	.zero	56

	/* #442 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554764
	/* java_name */
	.ascii	"android/view/animation/AccelerateInterpolator"
	.zero	57

	/* #443 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554765
	/* java_name */
	.ascii	"android/view/animation/Animation"
	.zero	70

	/* #444 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554767
	/* java_name */
	.ascii	"android/view/animation/Animation$AnimationListener"
	.zero	52

	/* #445 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554769
	/* java_name */
	.ascii	"android/view/animation/AnimationSet"
	.zero	67

	/* #446 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554770
	/* java_name */
	.ascii	"android/view/animation/AnimationUtils"
	.zero	65

	/* #447 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554771
	/* java_name */
	.ascii	"android/view/animation/BaseInterpolator"
	.zero	63

	/* #448 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554773
	/* java_name */
	.ascii	"android/view/animation/DecelerateInterpolator"
	.zero	57

	/* #449 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554775
	/* java_name */
	.ascii	"android/view/animation/Interpolator"
	.zero	67

	/* #450 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554776
	/* java_name */
	.ascii	"android/view/animation/LinearInterpolator"
	.zero	61

	/* #451 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554777
	/* java_name */
	.ascii	"android/view/inputmethod/InputMethodManager"
	.zero	59

	/* #452 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554499
	/* java_name */
	.ascii	"android/webkit/ValueCallback"
	.zero	74

	/* #453 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554503
	/* java_name */
	.ascii	"android/webkit/WebChromeClient"
	.zero	72

	/* #454 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554504
	/* java_name */
	.ascii	"android/webkit/WebChromeClient$FileChooserParams"
	.zero	54

	/* #455 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554506
	/* java_name */
	.ascii	"android/webkit/WebResourceError"
	.zero	71

	/* #456 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554501
	/* java_name */
	.ascii	"android/webkit/WebResourceRequest"
	.zero	69

	/* #457 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554508
	/* java_name */
	.ascii	"android/webkit/WebSettings"
	.zero	76

	/* #458 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554510
	/* java_name */
	.ascii	"android/webkit/WebView"
	.zero	80

	/* #459 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554511
	/* java_name */
	.ascii	"android/webkit/WebViewClient"
	.zero	74

	/* #460 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554518
	/* java_name */
	.ascii	"android/widget/AbsListView"
	.zero	76

	/* #461 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554520
	/* java_name */
	.ascii	"android/widget/AbsListView$OnScrollListener"
	.zero	59

	/* #462 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554545
	/* java_name */
	.ascii	"android/widget/AbsSeekBar"
	.zero	77

	/* #463 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554543
	/* java_name */
	.ascii	"android/widget/AbsoluteLayout"
	.zero	73

	/* #464 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554544
	/* java_name */
	.ascii	"android/widget/AbsoluteLayout$LayoutParams"
	.zero	60

	/* #465 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554569
	/* java_name */
	.ascii	"android/widget/Adapter"
	.zero	80

	/* #466 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554522
	/* java_name */
	.ascii	"android/widget/AdapterView"
	.zero	76

	/* #467 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554524
	/* java_name */
	.ascii	"android/widget/AdapterView$OnItemClickListener"
	.zero	56

	/* #468 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554528
	/* java_name */
	.ascii	"android/widget/AdapterView$OnItemLongClickListener"
	.zero	52

	/* #469 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554530
	/* java_name */
	.ascii	"android/widget/AdapterView$OnItemSelectedListener"
	.zero	53

	/* #470 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554532
	/* java_name */
	.ascii	"android/widget/AutoCompleteTextView"
	.zero	67

	/* #471 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"android/widget/BaseAdapter"
	.zero	76

	/* #472 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554551
	/* java_name */
	.ascii	"android/widget/Button"
	.zero	81

	/* #473 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554552
	/* java_name */
	.ascii	"android/widget/CheckBox"
	.zero	79

	/* #474 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554571
	/* java_name */
	.ascii	"android/widget/Checkable"
	.zero	78

	/* #475 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554554
	/* java_name */
	.ascii	"android/widget/CompoundButton"
	.zero	73

	/* #476 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554556
	/* java_name */
	.ascii	"android/widget/CompoundButton$OnCheckedChangeListener"
	.zero	49

	/* #477 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554536
	/* java_name */
	.ascii	"android/widget/DatePicker"
	.zero	77

	/* #478 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554538
	/* java_name */
	.ascii	"android/widget/DatePicker$OnDateChangedListener"
	.zero	55

	/* #479 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554558
	/* java_name */
	.ascii	"android/widget/EdgeEffect"
	.zero	77

	/* #480 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554559
	/* java_name */
	.ascii	"android/widget/EditText"
	.zero	79

	/* #481 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554560
	/* java_name */
	.ascii	"android/widget/Filter"
	.zero	81

	/* #482 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554562
	/* java_name */
	.ascii	"android/widget/Filter$FilterListener"
	.zero	66

	/* #483 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554563
	/* java_name */
	.ascii	"android/widget/Filter$FilterResults"
	.zero	67

	/* #484 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554573
	/* java_name */
	.ascii	"android/widget/Filterable"
	.zero	77

	/* #485 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554565
	/* java_name */
	.ascii	"android/widget/FrameLayout"
	.zero	76

	/* #486 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554566
	/* java_name */
	.ascii	"android/widget/FrameLayout$LayoutParams"
	.zero	63

	/* #487 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554567
	/* java_name */
	.ascii	"android/widget/HorizontalScrollView"
	.zero	67

	/* #488 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554576
	/* java_name */
	.ascii	"android/widget/ImageButton"
	.zero	76

	/* #489 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554577
	/* java_name */
	.ascii	"android/widget/ImageView"
	.zero	78

	/* #490 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554578
	/* java_name */
	.ascii	"android/widget/ImageView$ScaleType"
	.zero	68

	/* #491 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554584
	/* java_name */
	.ascii	"android/widget/LinearLayout"
	.zero	75

	/* #492 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554585
	/* java_name */
	.ascii	"android/widget/LinearLayout$LayoutParams"
	.zero	62

	/* #493 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554575
	/* java_name */
	.ascii	"android/widget/ListAdapter"
	.zero	76

	/* #494 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554586
	/* java_name */
	.ascii	"android/widget/ListView"
	.zero	79

	/* #495 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554587
	/* java_name */
	.ascii	"android/widget/NumberPicker"
	.zero	75

	/* #496 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554589
	/* java_name */
	.ascii	"android/widget/ProgressBar"
	.zero	76

	/* #497 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554590
	/* java_name */
	.ascii	"android/widget/RelativeLayout"
	.zero	73

	/* #498 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554591
	/* java_name */
	.ascii	"android/widget/RelativeLayout$LayoutParams"
	.zero	60

	/* #499 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554593
	/* java_name */
	.ascii	"android/widget/ScrollView"
	.zero	77

	/* #500 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554594
	/* java_name */
	.ascii	"android/widget/SearchView"
	.zero	77

	/* #501 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554596
	/* java_name */
	.ascii	"android/widget/SearchView$OnQueryTextListener"
	.zero	57

	/* #502 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554580
	/* java_name */
	.ascii	"android/widget/SectionIndexer"
	.zero	73

	/* #503 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554597
	/* java_name */
	.ascii	"android/widget/SeekBar"
	.zero	80

	/* #504 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554599
	/* java_name */
	.ascii	"android/widget/SeekBar$OnSeekBarChangeListener"
	.zero	56

	/* #505 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554582
	/* java_name */
	.ascii	"android/widget/SpinnerAdapter"
	.zero	73

	/* #506 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554600
	/* java_name */
	.ascii	"android/widget/Switch"
	.zero	81

	/* #507 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554539
	/* java_name */
	.ascii	"android/widget/TextView"
	.zero	79

	/* #508 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554540
	/* java_name */
	.ascii	"android/widget/TextView$BufferType"
	.zero	68

	/* #509 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554542
	/* java_name */
	.ascii	"android/widget/TextView$OnEditorActionListener"
	.zero	56

	/* #510 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554601
	/* java_name */
	.ascii	"android/widget/TimePicker"
	.zero	77

	/* #511 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554603
	/* java_name */
	.ascii	"android/widget/TimePicker$OnTimeChangedListener"
	.zero	55

	/* #512 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554604
	/* java_name */
	.ascii	"android/widget/Toast"
	.zero	82

	/* #513 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554529
	/* java_name */
	.ascii	"com/sewoo/image/android/AndroidImageLoader"
	.zero	60

	/* #514 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554531
	/* java_name */
	.ascii	"com/sewoo/image/android/ImageLoaderIF"
	.zero	65

	/* #515 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554478
	/* java_name */
	.ascii	"com/sewoo/jpos/POSPrinterService"
	.zero	70

	/* #516 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554514
	/* java_name */
	.ascii	"com/sewoo/jpos/command/CPCL"
	.zero	75

	/* #517 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554517
	/* java_name */
	.ascii	"com/sewoo/jpos/command/CPCLConst"
	.zero	70

	/* #518 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554519
	/* java_name */
	.ascii	"com/sewoo/jpos/command/EPLConst"
	.zero	71

	/* #519 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554515
	/* java_name */
	.ascii	"com/sewoo/jpos/command/EPOST"
	.zero	74

	/* #520 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554521
	/* java_name */
	.ascii	"com/sewoo/jpos/command/EPOSTConst"
	.zero	69

	/* #521 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554516
	/* java_name */
	.ascii	"com/sewoo/jpos/command/ESCPOS"
	.zero	73

	/* #522 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554523
	/* java_name */
	.ascii	"com/sewoo/jpos/command/ESCPOSConst"
	.zero	68

	/* #523 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554525
	/* java_name */
	.ascii	"com/sewoo/jpos/command/IcrMsrConst"
	.zero	68

	/* #524 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554527
	/* java_name */
	.ascii	"com/sewoo/jpos/command/ZPLConst"
	.zero	71

	/* #525 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554512
	/* java_name */
	.ascii	"com/sewoo/jpos/image/ImageLoader"
	.zero	70

	/* #526 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554513
	/* java_name */
	.ascii	"com/sewoo/jpos/image/MobileImageConverter"
	.zero	61

	/* #527 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554491
	/* java_name */
	.ascii	"com/sewoo/jpos/printer/CPCLPrinter"
	.zero	68

	/* #528 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554492
	/* java_name */
	.ascii	"com/sewoo/jpos/printer/CPCLWebPrinter"
	.zero	65

	/* #529 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554493
	/* java_name */
	.ascii	"com/sewoo/jpos/printer/EPLPrinter"
	.zero	69

	/* #530 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554494
	/* java_name */
	.ascii	"com/sewoo/jpos/printer/EPLWebPrinter"
	.zero	66

	/* #531 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554495
	/* java_name */
	.ascii	"com/sewoo/jpos/printer/EPOSTPrinter"
	.zero	67

	/* #532 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554496
	/* java_name */
	.ascii	"com/sewoo/jpos/printer/ESCPOSImage"
	.zero	68

	/* #533 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554497
	/* java_name */
	.ascii	"com/sewoo/jpos/printer/ESCPOSPrinter"
	.zero	66

	/* #534 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554498
	/* java_name */
	.ascii	"com/sewoo/jpos/printer/ESCPOSWebPrinter"
	.zero	63

	/* #535 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554500
	/* java_name */
	.ascii	"com/sewoo/jpos/printer/NFCPrinter"
	.zero	69

	/* #536 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554499
	/* java_name */
	.ascii	"com/sewoo/jpos/printer/NativeCall"
	.zero	69

	/* #537 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554501
	/* java_name */
	.ascii	"com/sewoo/jpos/printer/SpeedJNI"
	.zero	71

	/* #538 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554502
	/* java_name */
	.ascii	"com/sewoo/jpos/printer/ZPLPrinter"
	.zero	69

	/* #539 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554503
	/* java_name */
	.ascii	"com/sewoo/jpos/printer/ZPLRFIDPrinter"
	.zero	65

	/* #540 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554504
	/* java_name */
	.ascii	"com/sewoo/jpos/printer/ZPLWebPrinter"
	.zero	66

	/* #541 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554505
	/* java_name */
	.ascii	"com/sewoo/jpos/printer/lkp31/CompressJNI"
	.zero	62

	/* #542 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554506
	/* java_name */
	.ascii	"com/sewoo/jpos/printer/lkp31/FormFileLoader"
	.zero	59

	/* #543 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554507
	/* java_name */
	.ascii	"com/sewoo/jpos/printer/lkp31/FormPrinter"
	.zero	62

	/* #544 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554510
	/* java_name */
	.ascii	"com/sewoo/jpos/printer/lkp31/ImageOpener"
	.zero	62

	/* #545 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554508
	/* java_name */
	.ascii	"com/sewoo/jpos/printer/lkp31/LKP31Const"
	.zero	63

	/* #546 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554511
	/* java_name */
	.ascii	"com/sewoo/jpos/printer/lkp31/LKP31Printer"
	.zero	61

	/* #547 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554479
	/* java_name */
	.ascii	"com/sewoo/jpos/request/BlockingReadBleICR"
	.zero	61

	/* #548 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554480
	/* java_name */
	.ascii	"com/sewoo/jpos/request/BlockingReadBleICR$ReadStatusThread"
	.zero	44

	/* #549 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554481
	/* java_name */
	.ascii	"com/sewoo/jpos/request/BlockingReadICR"
	.zero	64

	/* #550 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554482
	/* java_name */
	.ascii	"com/sewoo/jpos/request/BlockingReadICR$ReadStatusThread"
	.zero	47

	/* #551 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554483
	/* java_name */
	.ascii	"com/sewoo/jpos/request/BlockingReadStatus"
	.zero	61

	/* #552 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554484
	/* java_name */
	.ascii	"com/sewoo/jpos/request/BlockingReadStatus$ReadStatusThread"
	.zero	44

	/* #553 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554485
	/* java_name */
	.ascii	"com/sewoo/jpos/request/BlockingReadStatusEPL"
	.zero	58

	/* #554 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554486
	/* java_name */
	.ascii	"com/sewoo/jpos/request/BlockingReadStatusEPL$ReadStatusThread"
	.zero	41

	/* #555 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554487
	/* java_name */
	.ascii	"com/sewoo/jpos/request/RequestData"
	.zero	68

	/* #556 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554488
	/* java_name */
	.ascii	"com/sewoo/jpos/request/RequestQueue"
	.zero	67

	/* #557 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554489
	/* java_name */
	.ascii	"com/sewoo/jpos/request/TimeOutChecker"
	.zero	65

	/* #558 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554490
	/* java_name */
	.ascii	"com/sewoo/jpos/request/TimeOutStripe"
	.zero	66

	/* #559 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554464
	/* java_name */
	.ascii	"com/sewoo/port/PortMediator"
	.zero	75

	/* #560 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554465
	/* java_name */
	.ascii	"com/sewoo/port/android/BluetoothPort"
	.zero	66

	/* #561 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554467
	/* java_name */
	.ascii	"com/sewoo/port/android/DeviceConnection"
	.zero	63

	/* #562 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554469
	/* java_name */
	.ascii	"com/sewoo/port/android/PortInterface"
	.zero	66

	/* #563 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554470
	/* java_name */
	.ascii	"com/sewoo/port/android/USBPort"
	.zero	72

	/* #564 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554471
	/* java_name */
	.ascii	"com/sewoo/port/android/USBPortConnection"
	.zero	62

	/* #565 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554472
	/* java_name */
	.ascii	"com/sewoo/port/android/USBPortConnection$ReadStatusThread"
	.zero	45

	/* #566 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554473
	/* java_name */
	.ascii	"com/sewoo/port/android/USBPortConnection$SenderThread"
	.zero	49

	/* #567 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554474
	/* java_name */
	.ascii	"com/sewoo/port/android/WiFiPort"
	.zero	71

	/* #568 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554475
	/* java_name */
	.ascii	"com/sewoo/port/android/WiFiPortConnection"
	.zero	61

	/* #569 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554476
	/* java_name */
	.ascii	"com/sewoo/port/android/WiFiPortConnection$ReadStatusThread"
	.zero	44

	/* #570 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554477
	/* java_name */
	.ascii	"com/sewoo/port/android/WiFiPortConnection$SenderThread"
	.zero	48

	/* #571 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554443
	/* java_name */
	.ascii	"com/sewoo/request/android/ARIAEngine"
	.zero	66

	/* #572 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554440
	/* java_name */
	.ascii	"com/sewoo/request/android/AndroidMSR"
	.zero	66

	/* #573 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554441
	/* java_name */
	.ascii	"com/sewoo/request/android/AndroidMSR$MSRReaderThread"
	.zero	50

	/* #574 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554442
	/* java_name */
	.ascii	"com/sewoo/request/android/AndroidMSR$MSRReaderThread$TimeChecker"
	.zero	38

	/* #575 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554444
	/* java_name */
	.ascii	"com/sewoo/request/android/BaseMSR"
	.zero	69

	/* #576 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554445
	/* java_name */
	.ascii	"com/sewoo/request/android/BaseMSR$MSRReaderTask"
	.zero	55

	/* #577 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554446
	/* java_name */
	.ascii	"com/sewoo/request/android/BaseMSR$MSRReaderTask$TimeChecker"
	.zero	43

	/* #578 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554449
	/* java_name */
	.ascii	"com/sewoo/request/android/DUKPTMSR"
	.zero	68

	/* #579 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554448
	/* java_name */
	.ascii	"com/sewoo/request/android/DebugLog"
	.zero	68

	/* #580 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554451
	/* java_name */
	.ascii	"com/sewoo/request/android/MultiConnector"
	.zero	62

	/* #581 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554452
	/* java_name */
	.ascii	"com/sewoo/request/android/RequestHandler"
	.zero	62

	/* #582 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554453
	/* java_name */
	.ascii	"com/sewoo/request/android/StandardPG"
	.zero	66

	/* #583 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554454
	/* java_name */
	.ascii	"com/sewoo/request/android/StandardPG$MSRReaderThread"
	.zero	50

	/* #584 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554455
	/* java_name */
	.ascii	"com/sewoo/request/android/StandardPG$MSRReaderThread$TimeChecker"
	.zero	38

	/* #585 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554456
	/* java_name */
	.ascii	"com/sewoo/request/android/UPlusPG"
	.zero	69

	/* #586 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554457
	/* java_name */
	.ascii	"com/sewoo/request/android/UPlusPG$MSRReaderThread"
	.zero	53

	/* #587 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554458
	/* java_name */
	.ascii	"com/sewoo/request/android/UPlusPG$MSRReaderThread$TimeChecker"
	.zero	41

	/* #588 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554459
	/* java_name */
	.ascii	"com/sewoo/request/android/UPlusVAN"
	.zero	68

	/* #589 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554460
	/* java_name */
	.ascii	"com/sewoo/request/android/UPlusVAN$MSRReaderThread"
	.zero	52

	/* #590 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554461
	/* java_name */
	.ascii	"com/sewoo/request/android/UPlusVAN$MSRReaderThread$TimeChecker"
	.zero	40

	/* #591 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554462
	/* java_name */
	.ascii	"com/sewoo/request/android/WiFiMultiConnector"
	.zero	58

	/* #592 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554463
	/* java_name */
	.ascii	"com/sewoo/request/android/WiFiMultiConnector$RequestMultiHandler"
	.zero	38

	/* #593 */
	/* module_index */
	.long	7
	/* type_token_id */
	.long	33554438
	/* java_name */
	.ascii	"com/xamarin/forms/platform/android/FormsViewGroup"
	.zero	53

	/* #594 */
	/* module_index */
	.long	7
	/* type_token_id */
	.long	33554440
	/* java_name */
	.ascii	"com/xamarin/formsviewgroup/BuildConfig"
	.zero	64

	/* #595 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"crc6414252951f3f66c67/RecyclerViewScrollListener_2"
	.zero	52

	/* #596 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554437
	/* java_name */
	.ascii	"crc6414fa209700c2b9f3/CachedImageFastRenderer"
	.zero	57

	/* #597 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554434
	/* java_name */
	.ascii	"crc6414fa209700c2b9f3/CachedImageRenderer"
	.zero	61

	/* #598 */
	/* module_index */
	.long	6
	/* type_token_id */
	.long	33554435
	/* java_name */
	.ascii	"crc6414fa209700c2b9f3/CachedImageView"
	.zero	65

	/* #599 */
	/* module_index */
	.long	18
	/* type_token_id */
	.long	33554437
	/* java_name */
	.ascii	"crc6427ea3917517e908b/ZXingBarcodeImageViewRenderer"
	.zero	51

	/* #600 */
	/* module_index */
	.long	18
	/* type_token_id */
	.long	33554434
	/* java_name */
	.ascii	"crc6427ea3917517e908b/ZXingScannerViewRenderer"
	.zero	56

	/* #601 */
	/* module_index */
	.long	20
	/* type_token_id */
	.long	33554436
	/* java_name */
	.ascii	"crc642b789bcd66d3f4e3/MainActivity"
	.zero	68

	/* #602 */
	/* module_index */
	.long	13
	/* type_token_id */
	.long	33554462
	/* java_name */
	.ascii	"crc643eead1a2954d3917/CameraEventsListener"
	.zero	60

	/* #603 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554607
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/AHorizontalScrollView"
	.zero	59

	/* #604 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554733
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ActionSheetRenderer"
	.zero	61

	/* #605 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554734
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ActivityIndicatorRenderer"
	.zero	55

	/* #606 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554541
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/AndroidActivity"
	.zero	65

	/* #607 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554543
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/BaseCellView"
	.zero	68

	/* #608 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554611
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/BorderDrawable"
	.zero	66

	/* #609 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554735
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/BoxRenderer"
	.zero	69

	/* #610 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554612
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ButtonRenderer"
	.zero	66

	/* #611 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554613
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ButtonRenderer_ButtonClickListener"
	.zero	46

	/* #612 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554615
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ButtonRenderer_ButtonTouchListener"
	.zero	46

	/* #613 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554767
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/CarouselPageAdapter"
	.zero	61

	/* #614 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554736
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/CarouselPageRenderer"
	.zero	60

	/* #615 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554463
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/CarouselSpacingItemDecoration"
	.zero	51

	/* #616 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554464
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/CarouselViewRenderer"
	.zero	60

	/* #617 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554519
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/CellAdapter"
	.zero	69

	/* #618 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554547
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/CellRenderer_RendererHolder"
	.zero	53

	/* #619 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554465
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/CenterSnapHelper"
	.zero	64

	/* #620 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554848
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/CheckBoxDesignerRenderer"
	.zero	56

	/* #621 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554847
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/CheckBoxRenderer"
	.zero	64

	/* #622 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554458
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/CheckBoxRendererBase"
	.zero	60

	/* #623 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554576
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/CircularProgress"
	.zero	64

	/* #624 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554617
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/CollectionViewRenderer"
	.zero	58

	/* #625 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554610
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ColorChangeRevealDrawable"
	.zero	55

	/* #626 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554618
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ConditionalFocusLayout"
	.zero	58

	/* #627 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554619
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ContainerView"
	.zero	67

	/* #628 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554620
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/CustomFrameLayout"
	.zero	63

	/* #629 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554466
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/DataChangeObserver"
	.zero	62

	/* #630 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554739
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/DatePickerRenderer"
	.zero	62

	/* #631 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/DatePickerRendererBase_1"
	.zero	56

	/* #632 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554481
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/EdgeSnapHelper"
	.zero	66

	/* #633 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554628
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/EditorEditText"
	.zero	66

	/* #634 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554740
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/EditorRenderer"
	.zero	66

	/* #635 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/EditorRendererBase_1"
	.zero	60

	/* #636 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554476
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/EmptyViewAdapter"
	.zero	64

	/* #637 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554478
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/EndSingleSnapHelper"
	.zero	61

	/* #638 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554482
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/EndSnapHelper"
	.zero	67

	/* #639 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554575
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/EntryAccessibilityDelegate"
	.zero	54

	/* #640 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554521
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/EntryCellEditText"
	.zero	63

	/* #641 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554522
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/EntryCellView"
	.zero	67

	/* #642 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554627
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/EntryEditText"
	.zero	67

	/* #643 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554742
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/EntryRenderer"
	.zero	67

	/* #644 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/EntryRendererBase_1"
	.zero	61

	/* #645 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554632
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/FormattedStringExtensions_FontSpan"
	.zero	46

	/* #646 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554634
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/FormattedStringExtensions_LineHeightSpan"
	.zero	40

	/* #647 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554633
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/FormattedStringExtensions_TextDecorationSpan"
	.zero	36

	/* #648 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554530
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/FormsAppCompatActivity"
	.zero	58

	/* #649 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554538
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/FormsApplicationActivity"
	.zero	56

	/* #650 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554623
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/FormsEditText"
	.zero	67

	/* #651 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554624
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/FormsEditTextBase"
	.zero	63

	/* #652 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554635
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/FormsImageView"
	.zero	66

	/* #653 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554812
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/FormsSeekBar"
	.zero	68

	/* #654 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554636
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/FormsTextView"
	.zero	67

	/* #655 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554637
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/FormsWebChromeClient"
	.zero	60

	/* #656 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554579
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/FormsWebViewClient"
	.zero	62

	/* #657 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554745
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/FrameRenderer"
	.zero	67

	/* #658 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554746
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/FrameRenderer_FrameDrawable"
	.zero	53

	/* #659 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554639
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/GenericAnimatorListener"
	.zero	57

	/* #660 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554715
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/GenericMenuClickListener"
	.zero	56

	/* #661 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554537
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/GestureManager_TapAndPanGestureDetector"
	.zero	41

	/* #662 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554472
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/GridLayoutSpanSizeLookup"
	.zero	56

	/* #663 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/GroupableItemsViewAdapter_2"
	.zero	53

	/* #664 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/GroupableItemsViewRenderer_3"
	.zero	52

	/* #665 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554816
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/GroupedListViewAdapter"
	.zero	58

	/* #666 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554535
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ImageButtonRenderer"
	.zero	61

	/* #667 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554565
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ImageCache_CacheEntry"
	.zero	59

	/* #668 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554566
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ImageCache_FormsLruCache"
	.zero	56

	/* #669 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554748
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ImageRenderer"
	.zero	67

	/* #670 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554582
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/InnerGestureListener"
	.zero	60

	/* #671 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554583
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/InnerScaleListener"
	.zero	62

	/* #672 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554485
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ItemContentView"
	.zero	65

	/* #673 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ItemsViewAdapter_2"
	.zero	62

	/* #674 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ItemsViewRenderer_3"
	.zero	61

	/* #675 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554753
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/LabelRenderer"
	.zero	67

	/* #676 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554754
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ListViewAdapter"
	.zero	65

	/* #677 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554756
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ListViewRenderer"
	.zero	64

	/* #678 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554757
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ListViewRenderer_Container"
	.zero	54

	/* #679 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554759
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ListViewRenderer_ListViewScrollDetector"
	.zero	41

	/* #680 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554758
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ListViewRenderer_SwipeRefreshLayoutWithFixedNestedScrolling"
	.zero	21

	/* #681 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554658
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/LocalizedDigitsKeyListener"
	.zero	54

	/* #682 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554659
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/MasterDetailContainer"
	.zero	59

	/* #683 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554761
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/MasterDetailRenderer"
	.zero	60

	/* #684 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554588
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/NativeViewWrapperRenderer"
	.zero	55

	/* #685 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554763
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/NavigationRenderer"
	.zero	62

	/* #686 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554473
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/NongreedySnapHelper"
	.zero	61

	/* #687 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ObjectJavaBox_1"
	.zero	65

	/* #688 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554808
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/OpenGLViewRenderer"
	.zero	62

	/* #689 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554809
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/OpenGLViewRenderer_Renderer"
	.zero	53

	/* #690 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554661
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/PageContainer"
	.zero	67

	/* #691 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554515
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/PageExtensions_EmbeddedFragment"
	.zero	49

	/* #692 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554517
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/PageExtensions_EmbeddedSupportFragment"
	.zero	42

	/* #693 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554768
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/PageRenderer"
	.zero	68

	/* #694 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554577
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/PickerEditText"
	.zero	66

	/* #695 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554574
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/PickerManager_PickerListener"
	.zero	52

	/* #696 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554802
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/PickerRenderer"
	.zero	66

	/* #697 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554731
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/PlatformRenderer"
	.zero	64

	/* #698 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554721
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/Platform_DefaultRenderer"
	.zero	56

	/* #699 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554490
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/PositionalSmoothScroller"
	.zero	56

	/* #700 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554606
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/PowerSaveModeBroadcastReceiver"
	.zero	50

	/* #701 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554770
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ProgressBarRenderer"
	.zero	61

	/* #702 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554849
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/RefreshViewRenderer"
	.zero	61

	/* #703 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554492
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ScrollHelper"
	.zero	68

	/* #704 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554662
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ScrollViewContainer"
	.zero	61

	/* #705 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554771
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ScrollViewRenderer"
	.zero	62

	/* #706 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554775
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/SearchBarRenderer"
	.zero	63

	/* #707 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/SelectableItemsViewAdapter_2"
	.zero	52

	/* #708 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/SelectableItemsViewRenderer_3"
	.zero	51

	/* #709 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554496
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/SelectableViewHolder"
	.zero	60

	/* #710 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554672
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ShellContentFragment"
	.zero	60

	/* #711 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554673
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ShellFlyoutRecyclerAdapter"
	.zero	54

	/* #712 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554676
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ShellFlyoutRecyclerAdapter_ElementViewHolder"
	.zero	36

	/* #713 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554674
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ShellFlyoutRecyclerAdapter_LinearLayoutWithFocus"
	.zero	32

	/* #714 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554677
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ShellFlyoutRenderer"
	.zero	61

	/* #715 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554678
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ShellFlyoutTemplatedContentRenderer"
	.zero	45

	/* #716 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554679
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ShellFlyoutTemplatedContentRenderer_HeaderContainer"
	.zero	29

	/* #717 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554681
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ShellFragmentPagerAdapter"
	.zero	55

	/* #718 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554665
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ShellItemRenderer"
	.zero	63

	/* #719 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554682
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ShellItemRendererBase"
	.zero	59

	/* #720 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554684
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ShellPageContainer"
	.zero	62

	/* #721 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554686
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ShellRenderer_SplitDrawable"
	.zero	53

	/* #722 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554688
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ShellSearchView"
	.zero	65

	/* #723 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554692
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ShellSearchViewAdapter"
	.zero	58

	/* #724 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554693
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ShellSearchViewAdapter_CustomFilter"
	.zero	45

	/* #725 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554694
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ShellSearchViewAdapter_ObjectWrapper"
	.zero	44

	/* #726 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554689
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ShellSearchView_ClipDrawableWrapper"
	.zero	45

	/* #727 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554701
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ShellSectionRenderer"
	.zero	60

	/* #728 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554697
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ShellToolbarTracker"
	.zero	61

	/* #729 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554698
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ShellToolbarTracker_FlyoutIconDrawerDrawable"
	.zero	36

	/* #730 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554474
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/SimpleViewHolder"
	.zero	64

	/* #731 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554475
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/SingleSnapHelper"
	.zero	64

	/* #732 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554497
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/SizedItemContentView"
	.zero	60

	/* #733 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554776
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/SliderRenderer"
	.zero	66

	/* #734 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554499
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/SpacingItemDecoration"
	.zero	59

	/* #735 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554500
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/StartSingleSnapHelper"
	.zero	59

	/* #736 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554501
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/StartSnapHelper"
	.zero	65

	/* #737 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554777
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/StepperRenderer"
	.zero	65

	/* #738 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554846
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/StepperRendererManager_StepperListener"
	.zero	42

	/* #739 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/StructuredItemsViewAdapter_2"
	.zero	52

	/* #740 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/StructuredItemsViewRenderer_3"
	.zero	51

	/* #741 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554524
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/SwitchCellView"
	.zero	66

	/* #742 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554778
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/SwitchRenderer"
	.zero	66

	/* #743 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554779
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/TabbedRenderer"
	.zero	66

	/* #744 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554780
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/TableViewModelRenderer"
	.zero	58

	/* #745 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554781
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/TableViewRenderer"
	.zero	63

	/* #746 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554504
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/TemplatedItemViewHolder"
	.zero	57

	/* #747 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554552
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/TextCellRenderer_TextCellView"
	.zero	51

	/* #748 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554505
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/TextViewHolder"
	.zero	66

	/* #749 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554783
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/TimePickerRenderer"
	.zero	62

	/* #750 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/TimePickerRendererBase_1"
	.zero	56

	/* #751 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554554
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ViewCellRenderer_ViewCellContainer"
	.zero	46

	/* #752 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554555
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ViewCellRenderer_ViewCellContainer_LongPressGestureListener"
	.zero	21

	/* #753 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554718
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ViewRenderer"
	.zero	68

	/* #754 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/ViewRenderer_2"
	.zero	66

	/* #755 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/VisualElementRenderer_1"
	.zero	57

	/* #756 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554801
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/VisualElementTracker_AttachTracker"
	.zero	46

	/* #757 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554784
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/WebViewRenderer"
	.zero	65

	/* #758 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554785
	/* java_name */
	.ascii	"crc643f46942d9dd1fff9/WebViewRenderer_JavascriptResult"
	.zero	48

	/* #759 */
	/* module_index */
	.long	8
	/* type_token_id */
	.long	33554455
	/* java_name */
	.ascii	"crc644bcdcf6d99873ace/FFAnimatedDrawable"
	.zero	62

	/* #760 */
	/* module_index */
	.long	8
	/* type_token_id */
	.long	33554453
	/* java_name */
	.ascii	"crc644bcdcf6d99873ace/FFBitmapDrawable"
	.zero	64

	/* #761 */
	/* module_index */
	.long	8
	/* type_token_id */
	.long	33554452
	/* java_name */
	.ascii	"crc644bcdcf6d99873ace/SelfDisposingBitmapDrawable"
	.zero	53

	/* #762 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554876
	/* java_name */
	.ascii	"crc64720bb2db43a66fe9/ButtonRenderer"
	.zero	66

	/* #763 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554899
	/* java_name */
	.ascii	"crc64720bb2db43a66fe9/CarouselPageRenderer"
	.zero	60

	/* #764 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"crc64720bb2db43a66fe9/FormsFragmentPagerAdapter_1"
	.zero	53

	/* #765 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554865
	/* java_name */
	.ascii	"crc64720bb2db43a66fe9/FormsViewPager"
	.zero	66

	/* #766 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554866
	/* java_name */
	.ascii	"crc64720bb2db43a66fe9/FragmentContainer"
	.zero	63

	/* #767 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554863
	/* java_name */
	.ascii	"crc64720bb2db43a66fe9/FrameRenderer"
	.zero	67

	/* #768 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554868
	/* java_name */
	.ascii	"crc64720bb2db43a66fe9/MasterDetailContainer"
	.zero	59

	/* #769 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554877
	/* java_name */
	.ascii	"crc64720bb2db43a66fe9/MasterDetailPageRenderer"
	.zero	56

	/* #770 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554879
	/* java_name */
	.ascii	"crc64720bb2db43a66fe9/NavigationPageRenderer"
	.zero	58

	/* #771 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554880
	/* java_name */
	.ascii	"crc64720bb2db43a66fe9/NavigationPageRenderer_ClickListener"
	.zero	44

	/* #772 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554881
	/* java_name */
	.ascii	"crc64720bb2db43a66fe9/NavigationPageRenderer_Container"
	.zero	48

	/* #773 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554882
	/* java_name */
	.ascii	"crc64720bb2db43a66fe9/NavigationPageRenderer_DrawerMultiplexedListener"
	.zero	32

	/* #774 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554897
	/* java_name */
	.ascii	"crc64720bb2db43a66fe9/PickerRenderer"
	.zero	66

	/* #775 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"crc64720bb2db43a66fe9/PickerRendererBase_1"
	.zero	60

	/* #776 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554870
	/* java_name */
	.ascii	"crc64720bb2db43a66fe9/Platform_ModalContainer"
	.zero	57

	/* #777 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554864
	/* java_name */
	.ascii	"crc64720bb2db43a66fe9/ShellFragmentContainer"
	.zero	58

	/* #778 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554889
	/* java_name */
	.ascii	"crc64720bb2db43a66fe9/SwitchRenderer"
	.zero	66

	/* #779 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554890
	/* java_name */
	.ascii	"crc64720bb2db43a66fe9/TabbedPageRenderer"
	.zero	62

	/* #780 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"crc64720bb2db43a66fe9/ViewRenderer_2"
	.zero	66

	/* #781 */
	/* module_index */
	.long	13
	/* type_token_id */
	.long	33554435
	/* java_name */
	.ascii	"crc6480997b3ef81bf9b2/ActivityLifecycleContextListener"
	.zero	48

	/* #782 */
	/* module_index */
	.long	13
	/* type_token_id */
	.long	33554448
	/* java_name */
	.ascii	"crc6480997b3ef81bf9b2/ZXingScannerFragment"
	.zero	60

	/* #783 */
	/* module_index */
	.long	13
	/* type_token_id */
	.long	33554449
	/* java_name */
	.ascii	"crc6480997b3ef81bf9b2/ZXingSurfaceView"
	.zero	64

	/* #784 */
	/* module_index */
	.long	13
	/* type_token_id */
	.long	33554446
	/* java_name */
	.ascii	"crc6480997b3ef81bf9b2/ZxingActivity"
	.zero	67

	/* #785 */
	/* module_index */
	.long	13
	/* type_token_id */
	.long	33554447
	/* java_name */
	.ascii	"crc6480997b3ef81bf9b2/ZxingOverlayView"
	.zero	64

	/* #786 */
	/* module_index */
	.long	3
	/* type_token_id */
	.long	33554457
	/* java_name */
	.ascii	"crc64a0e0a82d0db9a07d/ActivityLifecycleContextListener"
	.zero	48

	/* #787 */
	/* module_index */
	.long	8
	/* type_token_id */
	.long	33554471
	/* java_name */
	.ascii	"crc64b75d9ddab39d6c30/LRUCache"
	.zero	72

	/* #788 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554851
	/* java_name */
	.ascii	"crc64ee486da937c010f4/ButtonRenderer"
	.zero	66

	/* #789 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554854
	/* java_name */
	.ascii	"crc64ee486da937c010f4/FrameRenderer"
	.zero	67

	/* #790 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554861
	/* java_name */
	.ascii	"crc64ee486da937c010f4/ImageRenderer"
	.zero	67

	/* #791 */
	/* module_index */
	.long	1
	/* type_token_id */
	.long	33554859
	/* java_name */
	.ascii	"crc64ee486da937c010f4/LabelRenderer"
	.zero	67

	/* #792 */
	/* module_index */
	.long	8
	/* type_token_id */
	.long	33554464
	/* java_name */
	.ascii	"ffimageloading/cross/MvxCachedImageView"
	.zero	63

	/* #793 */
	/* module_index */
	.long	8
	/* type_token_id */
	.long	33554462
	/* java_name */
	.ascii	"ffimageloading/views/ImageViewAsync"
	.zero	67

	/* #794 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555350
	/* java_name */
	.ascii	"java/io/Closeable"
	.zero	85

	/* #795 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555346
	/* java_name */
	.ascii	"java/io/File"
	.zero	90

	/* #796 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555347
	/* java_name */
	.ascii	"java/io/FileDescriptor"
	.zero	80

	/* #797 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555348
	/* java_name */
	.ascii	"java/io/FileInputStream"
	.zero	79

	/* #798 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555352
	/* java_name */
	.ascii	"java/io/Flushable"
	.zero	85

	/* #799 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555355
	/* java_name */
	.ascii	"java/io/IOException"
	.zero	83

	/* #800 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555353
	/* java_name */
	.ascii	"java/io/InputStream"
	.zero	83

	/* #801 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555358
	/* java_name */
	.ascii	"java/io/OutputStream"
	.zero	82

	/* #802 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555360
	/* java_name */
	.ascii	"java/io/PrintWriter"
	.zero	83

	/* #803 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555361
	/* java_name */
	.ascii	"java/io/Reader"
	.zero	88

	/* #804 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555357
	/* java_name */
	.ascii	"java/io/Serializable"
	.zero	82

	/* #805 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555363
	/* java_name */
	.ascii	"java/io/StringWriter"
	.zero	82

	/* #806 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555364
	/* java_name */
	.ascii	"java/io/Writer"
	.zero	88

	/* #807 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555287
	/* java_name */
	.ascii	"java/lang/AbstractMethodError"
	.zero	73

	/* #808 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555288
	/* java_name */
	.ascii	"java/lang/AbstractStringBuilder"
	.zero	71

	/* #809 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555298
	/* java_name */
	.ascii	"java/lang/Appendable"
	.zero	82

	/* #810 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555300
	/* java_name */
	.ascii	"java/lang/AutoCloseable"
	.zero	79

	/* #811 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555266
	/* java_name */
	.ascii	"java/lang/Boolean"
	.zero	85

	/* #812 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555267
	/* java_name */
	.ascii	"java/lang/Byte"
	.zero	88

	/* #813 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555301
	/* java_name */
	.ascii	"java/lang/CharSequence"
	.zero	80

	/* #814 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555268
	/* java_name */
	.ascii	"java/lang/Character"
	.zero	83

	/* #815 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555269
	/* java_name */
	.ascii	"java/lang/Class"
	.zero	87

	/* #816 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555291
	/* java_name */
	.ascii	"java/lang/ClassCastException"
	.zero	74

	/* #817 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555292
	/* java_name */
	.ascii	"java/lang/ClassLoader"
	.zero	81

	/* #818 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555270
	/* java_name */
	.ascii	"java/lang/ClassNotFoundException"
	.zero	70

	/* #819 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555304
	/* java_name */
	.ascii	"java/lang/Cloneable"
	.zero	83

	/* #820 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555306
	/* java_name */
	.ascii	"java/lang/Comparable"
	.zero	82

	/* #821 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555271
	/* java_name */
	.ascii	"java/lang/Double"
	.zero	86

	/* #822 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555294
	/* java_name */
	.ascii	"java/lang/Enum"
	.zero	88

	/* #823 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555296
	/* java_name */
	.ascii	"java/lang/Error"
	.zero	87

	/* #824 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555272
	/* java_name */
	.ascii	"java/lang/Exception"
	.zero	83

	/* #825 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555273
	/* java_name */
	.ascii	"java/lang/Float"
	.zero	87

	/* #826 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555309
	/* java_name */
	.ascii	"java/lang/IllegalArgumentException"
	.zero	68

	/* #827 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555310
	/* java_name */
	.ascii	"java/lang/IllegalStateException"
	.zero	71

	/* #828 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555311
	/* java_name */
	.ascii	"java/lang/IncompatibleClassChangeError"
	.zero	64

	/* #829 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555312
	/* java_name */
	.ascii	"java/lang/IndexOutOfBoundsException"
	.zero	67

	/* #830 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555275
	/* java_name */
	.ascii	"java/lang/Integer"
	.zero	85

	/* #831 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555308
	/* java_name */
	.ascii	"java/lang/Iterable"
	.zero	84

	/* #832 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555318
	/* java_name */
	.ascii	"java/lang/LinkageError"
	.zero	80

	/* #833 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555276
	/* java_name */
	.ascii	"java/lang/Long"
	.zero	88

	/* #834 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555319
	/* java_name */
	.ascii	"java/lang/NoClassDefFoundError"
	.zero	72

	/* #835 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555320
	/* java_name */
	.ascii	"java/lang/NullPointerException"
	.zero	72

	/* #836 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555321
	/* java_name */
	.ascii	"java/lang/Number"
	.zero	86

	/* #837 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555277
	/* java_name */
	.ascii	"java/lang/Object"
	.zero	86

	/* #838 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555323
	/* java_name */
	.ascii	"java/lang/OutOfMemoryError"
	.zero	76

	/* #839 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555314
	/* java_name */
	.ascii	"java/lang/Readable"
	.zero	84

	/* #840 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555324
	/* java_name */
	.ascii	"java/lang/ReflectiveOperationException"
	.zero	64

	/* #841 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555316
	/* java_name */
	.ascii	"java/lang/Runnable"
	.zero	84

	/* #842 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555325
	/* java_name */
	.ascii	"java/lang/Runtime"
	.zero	85

	/* #843 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555278
	/* java_name */
	.ascii	"java/lang/RuntimeException"
	.zero	76

	/* #844 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555279
	/* java_name */
	.ascii	"java/lang/Short"
	.zero	87

	/* #845 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555280
	/* java_name */
	.ascii	"java/lang/String"
	.zero	86

	/* #846 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555282
	/* java_name */
	.ascii	"java/lang/StringBuffer"
	.zero	80

	/* #847 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555317
	/* java_name */
	.ascii	"java/lang/System"
	.zero	86

	/* #848 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555284
	/* java_name */
	.ascii	"java/lang/Thread"
	.zero	86

	/* #849 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555286
	/* java_name */
	.ascii	"java/lang/Throwable"
	.zero	83

	/* #850 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555326
	/* java_name */
	.ascii	"java/lang/UnsupportedOperationException"
	.zero	63

	/* #851 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555327
	/* java_name */
	.ascii	"java/lang/VirtualMachineError"
	.zero	73

	/* #852 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555329
	/* java_name */
	.ascii	"java/lang/Void"
	.zero	88

	/* #853 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555331
	/* java_name */
	.ascii	"java/lang/annotation/Annotation"
	.zero	71

	/* #854 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555332
	/* java_name */
	.ascii	"java/lang/reflect/AccessibleObject"
	.zero	68

	/* #855 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555336
	/* java_name */
	.ascii	"java/lang/reflect/AnnotatedElement"
	.zero	68

	/* #856 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555333
	/* java_name */
	.ascii	"java/lang/reflect/Executable"
	.zero	74

	/* #857 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555338
	/* java_name */
	.ascii	"java/lang/reflect/GenericDeclaration"
	.zero	66

	/* #858 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555340
	/* java_name */
	.ascii	"java/lang/reflect/Member"
	.zero	78

	/* #859 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555345
	/* java_name */
	.ascii	"java/lang/reflect/Method"
	.zero	78

	/* #860 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555342
	/* java_name */
	.ascii	"java/lang/reflect/Type"
	.zero	80

	/* #861 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555344
	/* java_name */
	.ascii	"java/lang/reflect/TypeVariable"
	.zero	72

	/* #862 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555183
	/* java_name */
	.ascii	"java/net/ConnectException"
	.zero	77

	/* #863 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555185
	/* java_name */
	.ascii	"java/net/HttpURLConnection"
	.zero	76

	/* #864 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555187
	/* java_name */
	.ascii	"java/net/InetSocketAddress"
	.zero	76

	/* #865 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555188
	/* java_name */
	.ascii	"java/net/Proxy"
	.zero	88

	/* #866 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555189
	/* java_name */
	.ascii	"java/net/Proxy$Type"
	.zero	83

	/* #867 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555190
	/* java_name */
	.ascii	"java/net/ProxySelector"
	.zero	80

	/* #868 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555192
	/* java_name */
	.ascii	"java/net/SocketAddress"
	.zero	80

	/* #869 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555194
	/* java_name */
	.ascii	"java/net/SocketException"
	.zero	78

	/* #870 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555195
	/* java_name */
	.ascii	"java/net/URI"
	.zero	90

	/* #871 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555196
	/* java_name */
	.ascii	"java/net/URL"
	.zero	90

	/* #872 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555197
	/* java_name */
	.ascii	"java/net/URLConnection"
	.zero	80

	/* #873 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555229
	/* java_name */
	.ascii	"java/nio/Buffer"
	.zero	87

	/* #874 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555233
	/* java_name */
	.ascii	"java/nio/ByteBuffer"
	.zero	83

	/* #875 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555230
	/* java_name */
	.ascii	"java/nio/CharBuffer"
	.zero	83

	/* #876 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555236
	/* java_name */
	.ascii	"java/nio/FloatBuffer"
	.zero	82

	/* #877 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555238
	/* java_name */
	.ascii	"java/nio/IntBuffer"
	.zero	84

	/* #878 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555243
	/* java_name */
	.ascii	"java/nio/channels/ByteChannel"
	.zero	73

	/* #879 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555245
	/* java_name */
	.ascii	"java/nio/channels/Channel"
	.zero	77

	/* #880 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555240
	/* java_name */
	.ascii	"java/nio/channels/FileChannel"
	.zero	73

	/* #881 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555247
	/* java_name */
	.ascii	"java/nio/channels/GatheringByteChannel"
	.zero	64

	/* #882 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555249
	/* java_name */
	.ascii	"java/nio/channels/InterruptibleChannel"
	.zero	64

	/* #883 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555251
	/* java_name */
	.ascii	"java/nio/channels/ReadableByteChannel"
	.zero	65

	/* #884 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555253
	/* java_name */
	.ascii	"java/nio/channels/ScatteringByteChannel"
	.zero	63

	/* #885 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555255
	/* java_name */
	.ascii	"java/nio/channels/SeekableByteChannel"
	.zero	65

	/* #886 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555258
	/* java_name */
	.ascii	"java/nio/channels/SelectableChannel"
	.zero	67

	/* #887 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555260
	/* java_name */
	.ascii	"java/nio/channels/SocketChannel"
	.zero	71

	/* #888 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555257
	/* java_name */
	.ascii	"java/nio/channels/WritableByteChannel"
	.zero	65

	/* #889 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555262
	/* java_name */
	.ascii	"java/nio/channels/spi/AbstractInterruptibleChannel"
	.zero	52

	/* #890 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555264
	/* java_name */
	.ascii	"java/nio/channels/spi/AbstractSelectableChannel"
	.zero	55

	/* #891 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555216
	/* java_name */
	.ascii	"java/security/KeyStore"
	.zero	80

	/* #892 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555218
	/* java_name */
	.ascii	"java/security/KeyStore$LoadStoreParameter"
	.zero	61

	/* #893 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555220
	/* java_name */
	.ascii	"java/security/KeyStore$ProtectionParameter"
	.zero	60

	/* #894 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555215
	/* java_name */
	.ascii	"java/security/Principal"
	.zero	79

	/* #895 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555221
	/* java_name */
	.ascii	"java/security/SecureRandom"
	.zero	76

	/* #896 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555222
	/* java_name */
	.ascii	"java/security/cert/Certificate"
	.zero	72

	/* #897 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555224
	/* java_name */
	.ascii	"java/security/cert/CertificateFactory"
	.zero	65

	/* #898 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555227
	/* java_name */
	.ascii	"java/security/cert/X509Certificate"
	.zero	68

	/* #899 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555226
	/* java_name */
	.ascii	"java/security/cert/X509Extension"
	.zero	70

	/* #900 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555177
	/* java_name */
	.ascii	"java/text/DecimalFormat"
	.zero	79

	/* #901 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555178
	/* java_name */
	.ascii	"java/text/DecimalFormatSymbols"
	.zero	72

	/* #902 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555181
	/* java_name */
	.ascii	"java/text/Format"
	.zero	86

	/* #903 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555179
	/* java_name */
	.ascii	"java/text/NumberFormat"
	.zero	80

	/* #904 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555142
	/* java_name */
	.ascii	"java/util/ArrayList"
	.zero	83

	/* #905 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555131
	/* java_name */
	.ascii	"java/util/Collection"
	.zero	82

	/* #906 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555199
	/* java_name */
	.ascii	"java/util/Dictionary"
	.zero	82

	/* #907 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555203
	/* java_name */
	.ascii	"java/util/Enumeration"
	.zero	81

	/* #908 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555133
	/* java_name */
	.ascii	"java/util/HashMap"
	.zero	85

	/* #909 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555151
	/* java_name */
	.ascii	"java/util/HashSet"
	.zero	85

	/* #910 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555201
	/* java_name */
	.ascii	"java/util/Hashtable"
	.zero	83

	/* #911 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555205
	/* java_name */
	.ascii	"java/util/Iterator"
	.zero	84

	/* #912 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555207
	/* java_name */
	.ascii	"java/util/Map"
	.zero	89

	/* #913 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555208
	/* java_name */
	.ascii	"java/util/Random"
	.zero	86

	/* #914 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555210
	/* java_name */
	.ascii	"java/util/concurrent/Executor"
	.zero	73

	/* #915 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555212
	/* java_name */
	.ascii	"java/util/concurrent/Future"
	.zero	75

	/* #916 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555213
	/* java_name */
	.ascii	"java/util/concurrent/TimeUnit"
	.zero	73

	/* #917 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554493
	/* java_name */
	.ascii	"javax/microedition/khronos/egl/EGLConfig"
	.zero	62

	/* #918 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554490
	/* java_name */
	.ascii	"javax/microedition/khronos/opengles/GL"
	.zero	64

	/* #919 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554492
	/* java_name */
	.ascii	"javax/microedition/khronos/opengles/GL10"
	.zero	62

	/* #920 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554468
	/* java_name */
	.ascii	"javax/net/SocketFactory"
	.zero	79

	/* #921 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554473
	/* java_name */
	.ascii	"javax/net/ssl/HostnameVerifier"
	.zero	72

	/* #922 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554470
	/* java_name */
	.ascii	"javax/net/ssl/HttpsURLConnection"
	.zero	70

	/* #923 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554475
	/* java_name */
	.ascii	"javax/net/ssl/KeyManager"
	.zero	78

	/* #924 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554484
	/* java_name */
	.ascii	"javax/net/ssl/KeyManagerFactory"
	.zero	71

	/* #925 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554485
	/* java_name */
	.ascii	"javax/net/ssl/SSLContext"
	.zero	78

	/* #926 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554477
	/* java_name */
	.ascii	"javax/net/ssl/SSLSession"
	.zero	78

	/* #927 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554479
	/* java_name */
	.ascii	"javax/net/ssl/SSLSessionContext"
	.zero	71

	/* #928 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554486
	/* java_name */
	.ascii	"javax/net/ssl/SSLSocketFactory"
	.zero	72

	/* #929 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554481
	/* java_name */
	.ascii	"javax/net/ssl/TrustManager"
	.zero	76

	/* #930 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554488
	/* java_name */
	.ascii	"javax/net/ssl/TrustManagerFactory"
	.zero	69

	/* #931 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554483
	/* java_name */
	.ascii	"javax/net/ssl/X509TrustManager"
	.zero	72

	/* #932 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554464
	/* java_name */
	.ascii	"javax/security/cert/Certificate"
	.zero	71

	/* #933 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554466
	/* java_name */
	.ascii	"javax/security/cert/X509Certificate"
	.zero	67

	/* #934 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554434
	/* java_name */
	.ascii	"jpos/JposConst"
	.zero	88

	/* #935 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554438
	/* java_name */
	.ascii	"jpos/JposException"
	.zero	84

	/* #936 */
	/* module_index */
	.long	12
	/* type_token_id */
	.long	33554436
	/* java_name */
	.ascii	"jpos/POSPrinterConst"
	.zero	82

	/* #937 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555387
	/* java_name */
	.ascii	"mono/android/TypeManager"
	.zero	78

	/* #938 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554991
	/* java_name */
	.ascii	"mono/android/animation/AnimatorEventDispatcher"
	.zero	56

	/* #939 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554996
	/* java_name */
	.ascii	"mono/android/animation/ValueAnimator_AnimatorUpdateListenerImplementor"
	.zero	32

	/* #940 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555024
	/* java_name */
	.ascii	"mono/android/app/DatePickerDialog_OnDateSetListenerImplementor"
	.zero	40

	/* #941 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555011
	/* java_name */
	.ascii	"mono/android/app/TabEventDispatcher"
	.zero	67

	/* #942 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555066
	/* java_name */
	.ascii	"mono/android/content/DialogInterface_OnCancelListenerImplementor"
	.zero	38

	/* #943 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555070
	/* java_name */
	.ascii	"mono/android/content/DialogInterface_OnClickListenerImplementor"
	.zero	39

	/* #944 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555073
	/* java_name */
	.ascii	"mono/android/content/DialogInterface_OnDismissListenerImplementor"
	.zero	37

	/* #945 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555127
	/* java_name */
	.ascii	"mono/android/runtime/InputStreamAdapter"
	.zero	63

	/* #946 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	0
	/* java_name */
	.ascii	"mono/android/runtime/JavaArray"
	.zero	72

	/* #947 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555148
	/* java_name */
	.ascii	"mono/android/runtime/JavaObject"
	.zero	71

	/* #948 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555166
	/* java_name */
	.ascii	"mono/android/runtime/OutputStreamAdapter"
	.zero	62

	/* #949 */
	/* module_index */
	.long	4
	/* type_token_id */
	.long	33554455
	/* java_name */
	.ascii	"mono/android/support/design/widget/AppBarLayout_OnOffsetChangedListenerImplementor"
	.zero	20

	/* #950 */
	/* module_index */
	.long	4
	/* type_token_id */
	.long	33554463
	/* java_name */
	.ascii	"mono/android/support/design/widget/BottomNavigationView_OnNavigationItemReselectedListenerImplementor"
	.zero	1

	/* #951 */
	/* module_index */
	.long	4
	/* type_token_id */
	.long	33554467
	/* java_name */
	.ascii	"mono/android/support/design/widget/BottomNavigationView_OnNavigationItemSelectedListenerImplementor"
	.zero	3

	/* #952 */
	/* module_index */
	.long	4
	/* type_token_id */
	.long	33554442
	/* java_name */
	.ascii	"mono/android/support/design/widget/TabLayout_BaseOnTabSelectedListenerImplementor"
	.zero	21

	/* #953 */
	/* module_index */
	.long	16
	/* type_token_id */
	.long	33554445
	/* java_name */
	.ascii	"mono/android/support/v4/app/FragmentManager_OnBackStackChangedListenerImplementor"
	.zero	21

	/* #954 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554456
	/* java_name */
	.ascii	"mono/android/support/v4/view/ActionProvider_SubUiVisibilityListenerImplementor"
	.zero	24

	/* #955 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554460
	/* java_name */
	.ascii	"mono/android/support/v4/view/ActionProvider_VisibilityListenerImplementor"
	.zero	29

	/* #956 */
	/* module_index */
	.long	23
	/* type_token_id */
	.long	33554441
	/* java_name */
	.ascii	"mono/android/support/v4/view/ViewPager_OnAdapterChangeListenerImplementor"
	.zero	29

	/* #957 */
	/* module_index */
	.long	23
	/* type_token_id */
	.long	33554447
	/* java_name */
	.ascii	"mono/android/support/v4/view/ViewPager_OnPageChangeListenerImplementor"
	.zero	32

	/* #958 */
	/* module_index */
	.long	11
	/* type_token_id */
	.long	33554442
	/* java_name */
	.ascii	"mono/android/support/v4/widget/DrawerLayout_DrawerListenerImplementor"
	.zero	33

	/* #959 */
	/* module_index */
	.long	15
	/* type_token_id */
	.long	33554447
	/* java_name */
	.ascii	"mono/android/support/v4/widget/NestedScrollView_OnScrollChangeListenerImplementor"
	.zero	21

	/* #960 */
	/* module_index */
	.long	25
	/* type_token_id */
	.long	33554440
	/* java_name */
	.ascii	"mono/android/support/v4/widget/SwipeRefreshLayout_OnRefreshListenerImplementor"
	.zero	24

	/* #961 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554443
	/* java_name */
	.ascii	"mono/android/support/v7/app/ActionBar_OnMenuVisibilityListenerImplementor"
	.zero	29

	/* #962 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554470
	/* java_name */
	.ascii	"mono/android/support/v7/widget/RecyclerView_OnChildAttachStateChangeListenerImplementor"
	.zero	15

	/* #963 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554478
	/* java_name */
	.ascii	"mono/android/support/v7/widget/RecyclerView_OnItemTouchListenerImplementor"
	.zero	28

	/* #964 */
	/* module_index */
	.long	2
	/* type_token_id */
	.long	33554486
	/* java_name */
	.ascii	"mono/android/support/v7/widget/RecyclerView_RecyclerListenerImplementor"
	.zero	31

	/* #965 */
	/* module_index */
	.long	22
	/* type_token_id */
	.long	33554471
	/* java_name */
	.ascii	"mono/android/support/v7/widget/Toolbar_OnMenuItemClickListenerImplementor"
	.zero	29

	/* #966 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554614
	/* java_name */
	.ascii	"mono/android/view/View_OnAttachStateChangeListenerImplementor"
	.zero	41

	/* #967 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554617
	/* java_name */
	.ascii	"mono/android/view/View_OnClickListenerImplementor"
	.zero	53

	/* #968 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554625
	/* java_name */
	.ascii	"mono/android/view/View_OnKeyListenerImplementor"
	.zero	55

	/* #969 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554629
	/* java_name */
	.ascii	"mono/android/view/View_OnLayoutChangeListenerImplementor"
	.zero	46

	/* #970 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554633
	/* java_name */
	.ascii	"mono/android/view/View_OnScrollChangeListenerImplementor"
	.zero	46

	/* #971 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554637
	/* java_name */
	.ascii	"mono/android/view/View_OnTouchListenerImplementor"
	.zero	53

	/* #972 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554526
	/* java_name */
	.ascii	"mono/android/widget/AdapterView_OnItemClickListenerImplementor"
	.zero	40

	/* #973 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33555285
	/* java_name */
	.ascii	"mono/java/lang/RunnableImplementor"
	.zero	68

	/* #974 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554461
	/* java_name */
	.ascii	"org/xmlpull/v1/XmlPullParser"
	.zero	74

	/* #975 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554462
	/* java_name */
	.ascii	"org/xmlpull/v1/XmlPullParserException"
	.zero	65

	/* #976 */
	/* module_index */
	.long	5
	/* type_token_id */
	.long	33554456
	/* java_name */
	.ascii	"xamarin/android/net/OldAndroidSSLSocketFactory"
	.zero	56

	.size	map_java, 107470
/* Java to managed map: END */

