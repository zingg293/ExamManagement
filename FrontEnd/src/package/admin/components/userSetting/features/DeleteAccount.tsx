import { Button, Card, Divider, Space, Switch, Typography } from "antd";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { HandleError } from "@admin/components";
import { useRemoveUserByListMutation } from "@API/services/UserApis.service";
import WithErrorBoundaryCustom from "@units/errorBounDary/WithErrorBoundaryCustom";
import { deleteCookie } from "~/units";

function _DeleteAccount({ idUser }: { idUser?: string }) {
  const navigate = useNavigate();
  const [removeUser, { isLoading }] = useRemoveUserByListMutation();
  const [isDelete, setIsDelete] = useState(false);
  const handleDelete = async () => {
    try {
      if (idUser) {
        const result = await removeUser({ listId: [idUser] }).unwrap();
        if (result.success) {
          deleteCookie("jwt");
          return navigate("/Print/user/setting/delete-account-success");
        }
      }
    } catch (e: any) {
      await HandleError(e);
    }
  };

  return (
    <div className="DeleteAccount">
      <Card bordered={false} className="criclebox h-full">
        <Typography.Title level={4}>Xóa tài khoản</Typography.Title>
        <Typography.Text type="danger">
          Xóa tài khoản sẽ xóa tất cả dữ liệu liên quan đến tài khoản của bạn. Bạn có chắc chắn muốn xóa tài khoản?
        </Typography.Text>
        <Divider />
        <Space direction="horizontal" size={"large"}>
          <Switch
            size="default"
            checkedChildren="Tôi chắc chắn"
            unCheckedChildren="Không chắc chắn"
            checked={isDelete}
            onClick={() => {
              setIsDelete(!isDelete);
            }}
          />
          <div className="delete-account-confirm">
            <Typography.Title level={4}>Xác nhận</Typography.Title>
            <Typography.Text>Tôi muốn xóa tài khoản của mình</Typography.Text>
          </div>
        </Space>
        <Divider />
        <div className="delete-account-btn" style={{ textAlign: "end" }}>
          <Button type="primary" loading={isLoading} danger disabled={!isDelete} onClick={() => handleDelete()}>
            Xóa tài khoản
          </Button>
        </div>
      </Card>
    </div>
  );
}

export const DeleteAccount = WithErrorBoundaryCustom(_DeleteAccount);
