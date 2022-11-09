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
        FILE_ADD_R = 9,
        FILE_ADD_A = 10,
        FILE_DELETE_R = 11,
        FILE_DELETE_A = 12
    }
}
