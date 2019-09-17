/////////////////////////////////////////////////////////////////////////////////////////////////////
//
// Audiokinetic Wwise generated include file. Do not edit.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////

#ifndef __WWISE_IDS_H__
#define __WWISE_IDS_H__

#include <AK/SoundEngine/Common/AkTypes.h>

namespace AK
{
    namespace EVENTS
    {
        static const AkUniqueID PLAYER_FOOTSTEPS = 1730208058U;
        static const AkUniqueID PLAYER_HEARTBEAT = 3484321931U;
        static const AkUniqueID SFX_AMBIENCE = 3583497273U;
        static const AkUniqueID SFX_BG = 4181511840U;
        static const AkUniqueID SFX_BUTTON = 125802325U;
        static const AkUniqueID SFX_FOOTSTEPS = 3364658470U;
        static const AkUniqueID SFX_MIXSOUND = 4230043762U;
        static const AkUniqueID SFX_SHOOT = 3036093774U;
        static const AkUniqueID SFX_STOPBG = 4040424632U;
        static const AkUniqueID VO_OYE = 1324836884U;
    } // namespace EVENTS

    namespace STATES
    {
        namespace PLAYERLIFE
        {
            static const AkUniqueID GROUP = 444815956U;

            namespace STATE
            {
                static const AkUniqueID ALIVE = 655265632U;
                static const AkUniqueID DEAD = 2044049779U;
            } // namespace STATE
        } // namespace PLAYERLIFE

    } // namespace STATES

    namespace SWITCHES
    {
        namespace MATERIAL
        {
            static const AkUniqueID GROUP = 3865314626U;

            namespace SWITCH
            {
                static const AkUniqueID DIRT = 2195636714U;
                static const AkUniqueID GRAVEL = 2185786256U;
                static const AkUniqueID METALSHEET = 675233303U;
                static const AkUniqueID WOODSOLID = 2219893051U;
            } // namespace SWITCH
        } // namespace MATERIAL

    } // namespace SWITCHES

    namespace GAME_PARAMETERS
    {
        static const AkUniqueID PLAYERHEALTH = 151362964U;
    } // namespace GAME_PARAMETERS

    namespace BANKS
    {
        static const AkUniqueID INIT = 1355168291U;
        static const AkUniqueID CHARACTERBANK = 2143169776U;
        static const AkUniqueID MYSOUNDBANK = 3104675574U;
        static const AkUniqueID MYVOICEBANK = 2057761113U;
    } // namespace BANKS

    namespace BUSSES
    {
        static const AkUniqueID AUDIOBUS = 1445131385U;
        static const AkUniqueID MASTER_AUDIO_BUS = 3803692087U;
        static const AkUniqueID MUSICBUS = 2886307548U;
        static const AkUniqueID VOICEBUS = 2045367873U;
    } // namespace BUSSES

    namespace AUDIO_DEVICES
    {
        static const AkUniqueID NO_OUTPUT = 2317455096U;
        static const AkUniqueID SYSTEM = 3859886410U;
    } // namespace AUDIO_DEVICES

}// namespace AK

#endif // __WWISE_IDS_H__
