import { createApi } from "@reduxjs/toolkit/query/react";
import { axiosBaseQuery } from "@API/http/axiosBaseQuery";
import { ListResponse, pagination, payloadResult } from "@models/common";
import { RoomDTO } from "@models/RoomDTO";

export const RoomApisService = createApi({
  reducerPath: "RoomApisService",
  tagTypes: ["RoomApisService"],
  baseQuery: axiosBaseQuery(),
  endpoints: (builder) => ({
    GetListRoom: builder.query<ListResponse<RoomDTO>, pagination>({
      query: ({ pageSize, pageNumber }): any => ({
        url: `/Room/getListRoom`,
        params: { pageSize, pageNumber }
      }),
      providesTags(result) {
        if (result && result.listPayload) {
          const { listPayload } = result;
          return [
            ...listPayload.map(({ id }) => ({ type: "RoomApisService" as const, id })),
            {
              type: "RoomApisService" as const,
              id: "LIST"
            }
          ];
        }
        return [{ type: "RoomApisService", id: "LIST" }];
      }
    }),
    UpdateRoom: builder.mutation<
      payloadResult,
      { Room: Partial<RoomDTO> }
    >({
      query: ({ Room }) => ({
        url: `/Room/updateRoom`,
        method: "PUT",
        data: Room
      }),
      // invalidatesTags: (result, error, arg) => (result ?
      //   [{ type: "RoomApisService", id: arg.Room.id }] : [])
      invalidatesTags: (result, error, arg) => {
        if (result && arg && arg.Room && arg.Room.id) {
          return [{ type: "RoomApisService", id: arg.Room.id }];
        }
        return [];
      }
    }),
    GetRoomById: builder.query<ListResponse<RoomDTO>, { idRoom: string }>({
      query: ({ idRoom }): any => ({
        url: `/Room/getRoomById`,
        params: { idRoom }
      }),
      providesTags: (result, error, arg) => [{ type: "RoomApisService", id: arg.idRoom }]
    }),
    InsertRoom: builder.mutation<
      payloadResult,
      { Room: Partial<RoomDTO> }
    >({
      query: ({ Room }) => ({
        url: `/Room/insertRoom`,
        method: "POST",
        data: Room
      }),
      invalidatesTags: (result) => (result ? [{ type: "RoomApisService", id: "LIST" }] : [])
    }),
    DeleteRoom: builder.mutation<payloadResult, { idRoom: string[] }>({
      query: ({ idRoom }) => ({
        url: `/Room/DeleteRoom`,
        method: "DELETE",
        data: idRoom
      }),
      invalidatesTags: (result) => (result ? [{ type: "RoomApisService", id: "LIST" }] : [])
    })
  })
});
export const {
  useGetListRoomQuery,
  useUpdateRoomMutation,
  useGetRoomByIdQuery,
  useInsertRoomMutation,
  useDeleteRoomMutation
} = RoomApisService;
