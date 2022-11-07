namespace CommandsKit
{
    public enum TypeCommand : byte
    {
        UNKNOW = 0,
        AUTHORIZATION_R = 1,
        AUTHORIZATION_A = 2,
        REGISTRATION_R = 3,
        REGISTRATION_A = 4,
        LS_R = 5,
        LS_A = 6,
        FILE_GET_R = 7,
        FILE_GET_A = 8,
        FILE_SEND_R = 9,
        FILE_SEND_A = 10
    }
}
