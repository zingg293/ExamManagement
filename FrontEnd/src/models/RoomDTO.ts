export interface RoomDTO {
  id: string;
  roomName?: string | null;
  isActive: boolean;
  createdDate?: Date | null;
  isOrder: boolean;
  fromDate?: Date | null;
  toDate?: Date | null;
  isDeleted: boolean;
}
